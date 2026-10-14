import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  playerCount = 2;
  gameCreated = false;

  selectPlayerCount(count: number): void {
    this.playerCount = count;
    this.gameCreated = false;
  }

  createGame(): void {
    this.gameCreated = true;
  }
}
