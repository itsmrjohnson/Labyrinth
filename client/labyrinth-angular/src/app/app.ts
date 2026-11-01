import { Component, computed, inject, signal } from '@angular/core';
import { GameService, GameResponse } from './services/game.service';
import { MazeTile } from './components/maze-tile/maze-tile';
import {
  InsertionPreviewCalculator,
  InsertionLocation,
  InsertionPreview,
} from './game/insertion-preview-calculator';

@Component({
  selector: 'app-root',
  imports: [MazeTile],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  private readonly gameService = inject(GameService);

  /**
   * Converts the rotation name received from the API into degrees.
   *
   * The API returns the C# TileRotation enum as a string.
   * Angular uses degrees to display the spare tile's rotation.
   *
   * For example, 'Ninety' becomes 90 degrees.
   */
  private rotationToDegrees(rotation: string): number {
    const rotations: Record<string, number> = {
      Zero: 0,
      Ninety: 90,
      OneEighty: 180,
      TwoSeventy: 270,
    };

    return rotations[rotation] ?? 0;
  }

  playerCount = 2;

  gameCreated = signal(false);
  isLoading = signal(false);
  errorMessage = signal('');
  game = signal<GameResponse | null>(null);
  previewRotation = signal(0);
  animationRotation = 0;

  // The insertion location and index currently selected for preview.
  insertionLocation = signal<InsertionLocation>('Left');

  // The row or column where the insertion will happen.
  insertionIndex = signal(1);

  selectPlayerCount(count: number): void {
    this.playerCount = count;
    this.gameCreated.set(false);
    this.game.set(null);
    this.errorMessage.set('');
  }

  createGame(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.game.set(null);

    this.gameService
      .createGame({
        playerCount: this.playerCount,
      })
      .subscribe({
        next: (response) => {
          this.game.set(response);
          this.gameCreated.set(true);
          this.isLoading.set(false);
          this.previewRotation.set(this.rotationToDegrees(response.spareTile.rotation));
          this.animationRotation = this.previewRotation();
          console.log('Game created:', response);
          console.log('Game ID:', response.gameId);
        },
        error: (error) => {
          console.error('Failed to create game:', error);

          this.errorMessage.set(
            'Could not create the game. Check that the API is running and try again.',
          );

          this.isLoading.set(false);
        },
      });
  }

  /**
   * Rotates the spare tile 90 degrees clockwise.
   *
   * We add 90 to the current preview rotation to turn the tile
   * clockwise by one quarter turn.
   *
   * The modulo operator (%) divides the result by 360 and returns
   * the remainder. This keeps the rotation within the range of
   * 0, 90, 180 and 270 degrees.
   *
   * For example:
   *  - 0 + 90   = 90
   *  - 90 + 90  = 180
   *  - 180 + 90 = 270
   *  - 270 + 90 = 360 % 360 = 0
   *
   * This means the rotation automatically returns to 0 degrees
   * after completing a full 360-degree rotation.
   *
   * previewRotation tracks the tile's actual orientation (0, 90, 180 or 270).
   * animationRotation tracks the total rotation so CSS always animates
   * in the correct direction without jumping back to zero.
   */
  rotateSpareTileClockwise(): void {
    this.previewRotation.set((this.previewRotation() + 90) % 360);
    this.animationRotation += 90;
  }

  /**
   * Rotates the spare tile 90 degrees counter-clockwise.
   *
   * Instead of subtracting 90, we add 270 degrees.
   * This is mathematically equivalent because a full rotation
   * is 360 degrees, and 360 - 90 = 270.
   *
   * Using addition avoids negative rotation values, which would
   * otherwise need extra handling.
   *
   * The modulo operator (%) ensures the rotation stays within
   * 0, 90, 180 and 270 degrees.
   *
   * For example:
   *  - 0 + 270   = 270
   *  - 270 + 270 = 540 % 360 = 180
   *  - 180 + 270 = 450 % 360 = 90
   *  - 90 + 270  = 360 % 360 = 0
   *
   * This means the rotation automatically returns to 0 degrees
   * after completing a full rotation in the opposite direction.
   *
   * Subtracting 90 from animationRotation ensures the animation
   * moves counter-clockwise, even when crossing the zero-degree boundary
   */
  rotateSpareTileCounterClockwise(): void {
    this.previewRotation.set((this.previewRotation() + 270) % 360);
    this.animationRotation -= 90;
  }

  // Calculates the board changes for an insertion preview.
  private readonly insertionPreviewCalculator = new InsertionPreviewCalculator();

  // Whether the player is currently previewing an insertion.
  insertionPreviewActive = signal(false);

  /**
   * Calculates the preview from the current game state.
   *
   * This is separate from the actual game signal, so previewing
   * an insertion does not modify the real game.
   */
  insertionPreview = computed<InsertionPreview | null>(() => {
    const currentGame = this.game();

    if (!currentGame || !this.insertionPreviewActive()) {
      return null;
    }

    return this.insertionPreviewCalculator.calculate(
      currentGame.tiles,
      currentGame.spareTile,
      currentGame.playerPositions,
      this.insertionLocation(),
      this.insertionIndex(),
      this.previewRotation(),
    );
  });

  /**
   * Returns the tiles that the existing board should display.
   *
   * During a preview, use the calculated tile positions.
   * Otherwise, display the actual game tiles.
   */
  displayedTiles = computed(() => {
    return this.insertionPreview()?.tiles ?? this.game()?.tiles ?? [];
  });

  /**
   * Returns the player positions that the existing board should display.
   */
  displayedPlayerPositions = computed(() => {
    return this.insertionPreview()?.playerPositions ?? this.game()?.playerPositions ?? [];
  });

  /**
   * Selects the side of the board where the spare tile will enter.
   * Selecting a side activates the insertion preview.
   */
  selectInsertionLocation(location: InsertionLocation): void {
    this.insertionLocation.set(location);
    this.insertionPreviewActive.set(true);
  }

  /**
   * Selects the row or column that will be shifted.
   * Selecting an index also activates the insertion preview.
   */
  selectInsertionIndex(index: number): void {
    this.insertionIndex.set(index);
    this.insertionPreviewActive.set(true);
  }
}
