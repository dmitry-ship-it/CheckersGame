import { startGame } from "../App";
import { playerName } from "../header/navbar";
import PendingItemDescription from "./pending-item-description";
import ApiRouter from "./utils/router";
import { PendingGame } from "./utils/types";

export default function PendingItem(ctx: { game: PendingGame }) {
  return (
    <div className="bg-fuchsia-700 hover:bg-pink-700 text-gray-200 m-2 px-3 py-1 rounded-xl flex flex-wrap justify-between">
      <PendingItemDescription game={ctx.game} />
      <button className="bg-sky-600 text-white font-semibold leading-10 px-3 place-self-center rounded-xl" onClick={() => {}}>
        Spectate
      </button>
      {ctx.game.isPending ? (
        <button
          className="bg-green-600 text-white font-semibold leading-10 px-3 place-self-center rounded-xl"
          onClick={async () => {
            startGame(
              await ApiRouter.post("join", {
                gameId: ctx.game.gameId,
                secondPlayerName: playerName,
              })
            );
          }}>
          Join
        </button>
      ) : null}
    </div>
  );
}
