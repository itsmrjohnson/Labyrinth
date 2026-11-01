import { GameTile, Position } from '../services/game.service';

export type InsertionLocation = 'Top' | 'Right' | 'Bottom' | 'Left';

export interface InsertionPreview {
  tiles: GameTile[];
  playerPositions: Position[];
  outgoingTile: GameTile;
}

/**
 * Calculates what the board would look like after a tile insertion.
 *
 * This class does not change the actual game state. It creates new
 * tile and player position data for the UI to display as a preview.
 */
export class InsertionPreviewCalculator {
  private readonly boardSize = 7;

  /**
   * Creates the complete insertion preview.
   */
  calculate(
    tiles: GameTile[],
    spareTile: GameTile,
    playerPositions: Position[],
    location: InsertionLocation,
    index: number,
    previewRotation: number,
  ): InsertionPreview {
    const outgoingTile = this.getOutgoingTile(tiles, location, index);

    const incomingTile = this.createIncomingTile(spareTile, previewRotation);

    return {
      tiles: this.calculatePreviewTiles(tiles, incomingTile, location, index),
      playerPositions: this.calculatePreviewPlayerPositions(playerPositions, location, index),
      outgoingTile,
    };
  }

  /**
   * Creates a rotated copy of the spare tile.
   * The original spare tile is left unchanged.
   */
  private createIncomingTile(spareTile: GameTile, rotationDegrees: number): GameTile {
    return {
      ...spareTile,
      rotation: this.degreesToRotation(rotationDegrees),
    };
  }

  /**
   * Moves the existing tiles and adds the incoming tile.
   */
  private calculatePreviewTiles(
    tiles: GameTile[],
    incomingTile: GameTile,
    location: InsertionLocation,
    index: number,
  ): GameTile[] {
    const previewTiles: GameTile[] = [];

    for (const tile of tiles) {
      // The outgoing tile leaves the board and becomes the new spare tile.
      if (this.isOutgoingTile(tile, location, index)) {
        continue;
      }

      previewTiles.push(this.getMovedTile(tile, location, index));
    }

    // Add the incoming tile at the newly opened position.
    previewTiles.push({
      ...incomingTile,
      ...this.getIncomingPosition(location, index),
    });

    return previewTiles;
  }

  /**
   * Returns a tile at its new position after insertion.
   * Tiles outside the selected row or column remain where they are.
   */
  private getMovedTile(tile: GameTile, location: InsertionLocation, index: number): GameTile {
    switch (location) {
      case 'Left':
        return tile.row === index ? { ...tile, column: tile.column + 1 } : tile;

      case 'Right':
        return tile.row === index ? { ...tile, column: tile.column - 1 } : tile;

      case 'Top':
        return tile.column === index ? { ...tile, row: tile.row + 1 } : tile;

      case 'Bottom':
        return tile.column === index ? { ...tile, row: tile.row - 1 } : tile;
    }
  }

  /**
   * Finds the tile that will be pushed off the board.
   */
  private getOutgoingTile(tiles: GameTile[], location: InsertionLocation, index: number): GameTile {
    const outgoingTile = tiles.find((tile) => this.isOutgoingTile(tile, location, index));

    if (!outgoingTile) {
      throw new Error('Could not find the outgoing tile.');
    }

    return outgoingTile;
  }

  /**
   * Checks whether a tile is at the outgoing end of the selected row
   * or column.
   */
  private isOutgoingTile(tile: GameTile, location: InsertionLocation, index: number): boolean {
    switch (location) {
      case 'Left':
        return tile.row === index && tile.column === this.boardSize - 1;

      case 'Right':
        return tile.row === index && tile.column === 0;

      case 'Top':
        return tile.column === index && tile.row === this.boardSize - 1;

      case 'Bottom':
        return tile.column === index && tile.row === 0;
    }
  }

  /**
   * Determines where the incoming spare tile will appear.
   */
  private getIncomingPosition(location: InsertionLocation, index: number): Position {
    switch (location) {
      case 'Left':
        return { row: index, column: 0 };

      case 'Right':
        return { row: index, column: this.boardSize - 1 };

      case 'Top':
        return { row: 0, column: index };

      case 'Bottom':
        return { row: this.boardSize - 1, column: index };
    }
  }

  /**
   * Calculates the new positions of all players.
   * Players on the selected row or column move with the tiles.
   * Players at the outgoing edge wrap around to the opposite edge.
   */
  private calculatePreviewPlayerPositions(
    playerPositions: Position[],
    location: InsertionLocation,
    index: number,
  ): Position[] {
    return playerPositions.map((position) => {
      switch (location) {
        case 'Left':
          return position.row === index
            ? {
                ...position,
                column: (position.column + 1) % this.boardSize,
              }
            : position;

        case 'Right':
          return position.row === index
            ? {
                ...position,
                column: (position.column + this.boardSize - 1) % this.boardSize,
              }
            : position;

        case 'Top':
          return position.column === index
            ? {
                ...position,
                row: (position.row + 1) % this.boardSize,
              }
            : position;

        case 'Bottom':
          return position.column === index
            ? {
                ...position,
                row: (position.row + this.boardSize - 1) % this.boardSize,
              }
            : position;
      }
    });
  }

  /**
   * Converts degrees into the rotation string used by the API.
   */
  private degreesToRotation(degrees: number): string {
    const rotations: Record<number, string> = {
      0: 'Zero',
      90: 'Ninety',
      180: 'OneEighty',
      270: 'TwoSeventy',
    };

    const rotation = rotations[degrees];

    if (rotation === undefined) {
      throw new Error(`Invalid preview rotation: ${degrees}`);
    }

    return rotation;
  }
}
