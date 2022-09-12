import { startGame } from "../App";
import { playerName } from "../header/navbar";
import ApiRouter from "./utils/router";
import { PendingGame } from "./utils/types";

export default function PendingItem(game: PendingGame) {
  return (
    <div className="bg-neutral-500 text-gray-200 m-2 px-3 rounded-lg flex flex-wrap justify-between">
      <div className="mr-20">
        <div>Game type: {game.gameType}</div>
        <div>
          <span className="text-white font-semibold">{game.firstPlayerName}</span> is waiting for opponent
        </div>
      </div>
      <button
        className="bg-green-600 text-white font-semibold h-9 px-3 place-self-center rounded-lg"
        onClick={async () => {
          startGame(
            await ApiRouter.post("join", {
              gameId: game.gameId,
              secondPlayerName: playerName,
            })
          );
        }}>
        Join
      </button>
    </div>
  );
}
