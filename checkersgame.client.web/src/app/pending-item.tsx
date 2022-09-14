import { startGame } from "../App";
import { playerName } from "../header/navbar";
import PendingItemDescription from "./pending-item-description";
import ApiRouter from "./utils/router";
import { PendingGame } from "./utils/types";

export default function PendingItem(game: PendingGame) {
  return (
    <div className="bg-fuchsia-700 hover:bg-pink-700 text-gray-200 m-2 px-3 py-1 rounded-xl flex flex-wrap justify-between">
      <PendingItemDescription gameType={game.gameType} firstPlayerName={game.firstPlayerName} />
      <button className="bg-sky-600 text-white font-semibold leading-10 px-3 place-self-center rounded-xl" onClick={() => {}}>
        Spectate
      </button>
      <button
        className="bg-green-600 text-white font-semibold leading-10 px-3 place-self-center rounded-xl"
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
