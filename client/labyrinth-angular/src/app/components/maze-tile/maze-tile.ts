import { Component, input } from '@angular/core';

@Component({
  selector: 'app-maze-tile',
  standalone: true,
  templateUrl: './maze-tile.html',
  styleUrl: './maze-tile.scss',
})
export class MazeTile {
  type = input.required<string>();
  orientation = input.required<number>();
  player = input<number | null>(null);
}
