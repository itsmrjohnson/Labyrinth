import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreateGameRequest {
  playerCount: number;
}

export interface Position {
  row: number;
  column: number;
}

export interface GameTile {
  row: number;
  column: number;
  type: string;
  rotation: string;
  movement: string;
  connections: string[];
}

export interface GameResponse {
  gameId: string;
  currentPlayerIndex: number;
  turnPhase: string;
  playerPositions: Position[];
  tiles: GameTile[];
  spareTile: GameTile;
}

export interface InsertSpareTileRequest {
  location: 'Top' | 'Right' | 'Bottom' | 'Left';
  index: number;
  rotation: 'Zero' | 'Ninety' | 'OneEighty' | 'TwoSeventy';
}

@Injectable({ providedIn: 'root' })
export class GameService {
  private readonly http = inject(HttpClient);

  // Replace this with the HTTPS URL printed when you start the API.
  private readonly apiUrl = 'http://localhost:5148/api/games';

  createGame(request: CreateGameRequest): Observable<GameResponse> {
    return this.http.post<GameResponse>(this.apiUrl, request);
  }

  insertSpareTile(gameId: string, request: InsertSpareTileRequest): Observable<GameResponse> {
    return this.http.post<GameResponse>(`${this.apiUrl}/${gameId}/insert`, request);
  }
}
