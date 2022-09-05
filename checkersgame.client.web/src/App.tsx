import { useState } from "react";
//import logo from "./logo.svg";
import "./App.css";
import PendingList, { CurrentGame } from "./app/pending-list";
import GameField from "./app/GameField";

let setGameFunc: any;

export const startGame = (gameModel: CurrentGame) => {
  setGameFunc(gameModel);
};

export default function App() {

  const [game, setGame] = useState<CurrentGame | null>(null);

  setGameFunc = setGame;

  return game == null ? (
    <PendingList />
  ) : (
    <GameField board={game.board} currentPlayerTurn={game.currentPlayerTurn} id={game.id} isEnded={game.isEnded} playerId={game.playerId} />
  );
}
