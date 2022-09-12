import { useState } from "react";
import "./App.css";
import { Game } from "./app/utils/types";
import GameField from "./app/game-field";
import PendingList from "./app/pending-list";

let setGameFunc: any;

export const startGame = (gameModel: Game) => {
  setGameFunc(gameModel);
};

export default function App() {
  const [game, setGame] = useState<Game | null>(null);

  setGameFunc = setGame;

  return game == null ? (
    <PendingList />
  ) : (
    <GameField board={game.board} currentPlayerTurn={game.currentPlayerTurn} id={game.id} isEnded={game.isEnded} playerId={game.playerId} />
  );
}
