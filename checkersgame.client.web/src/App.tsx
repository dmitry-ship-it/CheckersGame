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
  document.body.classList.add("bg-gradient-to-tr", "from-pink-300", "to-amber-200", "bg-no-repeat");

  return (
    <div className="flex-auto container mx-auto py-16 text-lg">
      {game === null ? (
        <PendingList />
      ) : (
        <GameField
          board={game.board}
          firstPlayerName={game.firstPlayerName}
          secondPlayerName={game.secondPlayerName}
          currentPlayerTurn={game.currentPlayerTurn}
          id={game.id}
          isEnded={game.isEnded}
          playerId={game.playerId}
        />
      )}
    </div>
  );
}
