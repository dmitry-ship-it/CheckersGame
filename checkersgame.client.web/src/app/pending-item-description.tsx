import { PendingGame } from "./utils/types";

export default function PendingItemDescription(ctx: { game: PendingGame }) {
  return (
    <div className="mr-12">
      <div>Game type: {ctx.game.gameType}</div>
      <div>
        {ctx.game.isPending ? (
          <div>
            <span className="text-white font-semibold">{ctx.game.firstPlayerName}</span>
            <span> is waiting for opponent</span>
          </div>
        ) : (
          <div>
            <span className="text-white font-semibold">{ctx.game.secondPlayerName}</span>
            <span> is fighting with </span>
            <span className="text-white font-semibold">{ctx.game.firstPlayerName}</span>
          </div>
        )}
      </div>
    </div>
  );
}
