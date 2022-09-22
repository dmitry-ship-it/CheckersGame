import { PendingGame } from "./utils/types";

export default function PendingItemDescription(ctx: { game: PendingGame }) {
  return (
    <div className="mr-20">
      <div>Game type: {ctx.game.gameType}</div>
      <div>
        {ctx.game.isPending ? (
          <div>
            <span className="text-white font-semibold">{ctx.game.firstPlayerName}</span> is waiting for opponent
          </div>
        ) : (
          <div>
            <span className="text-white font-semibold">{ctx.game.secondPlayerName}</span> is fighting with{" "}
            <span className="text-white font-semibold">{ctx.game.firstPlayerName}</span>
          </div>
        )}
      </div>
    </div>
  );
}
