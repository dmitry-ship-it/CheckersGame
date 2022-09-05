import { startGame } from "../App";
import { playerName } from "../top/navbar";
import { PendingGame } from "./pending-list";

export interface JoinModel {
  gameId: string;
  secondPlayerName: string;
}

let pendingGame: PendingGame;

const sendJoinRequest = async () => {

  let joinModel: JoinModel = {
    gameId: pendingGame.gameId,
    secondPlayerName: playerName
  }

  const response = await fetch("https://localhost:7167/api/game/join", {
    method: "POST",
    headers: {
      "Content-type": "application/json; charset=UTF-8",
    },
    body: JSON.stringify(joinModel)
  })

  startGame(await response.json());
}

export default function PendingItem(game: PendingGame) {
  pendingGame = game;

  return (
    <div className="bg-neutral-500 text-gray-200 m-2 px-3 rounded-lg flex flex-wrap justify-between">
      <div className="mr-20">
        <div>Game type: {game.gameType}</div>
        <div>
          <span className="text-white font-semibold">{game.firstPlayerName}</span> is waiting for opponent
        </div>
      </div>
      <button className="bg-green-600 text-white font-semibold h-9 px-3 place-self-center rounded-lg"
              onClick={sendJoinRequest}>Join</button>
    </div>
  );
}
