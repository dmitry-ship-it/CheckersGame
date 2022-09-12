export interface SelectedCells {
  first: Element | null;
  second: Element | null;
}

export interface JoinModel {
  gameId: string;
  secondPlayerName: string;
}

export interface StepGameModel {
  gameId: string;
  playerId: string;
  from: {
    row: number;
    col: number;
  };
  to: {
    row: number;
    col: number;
  };
}

export interface UpdateGameModel {
  gameId: string;
  playerId: string;
  board: (string | null)[];
}

export interface PendingGame {
  gameId: string;
  gameType: string;
  firstPlayerName: string;
}

export interface NewGameRequestModel {
  gameType: string;
  playerName: string;
}

export interface Game {
  id: string;
  playerId: string;
  board: (string | null)[];
  currentPlayerTurn: string;
  isEnded: boolean;
}
