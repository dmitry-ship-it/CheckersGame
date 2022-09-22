import PendingItem from "./pending-item";
import { useEffect, useState } from "react";
import { PendingGame } from "./utils/types";
import ApiRouter from "./utils/router";
import AddNewGame from "./game-add-controls";

let isGamesAutoUpdating = false;

export default function PendingList() {
  // pending games loading and updating
  const [pendingGames, setPendingGames] = useState<PendingGame[]>([]);
  useEffect(() => {
    if (!isGamesAutoUpdating) {
      isGamesAutoUpdating = true;
      ApiRouter.createLongPollingPost("list", pendingGames, setPendingGames);
    }
  }, [pendingGames]);

  return (
    <div className="border-2 rounded-xl p-2 max-w-xl mx-auto flex flex-col">
      {pendingGames.map((game) => {
        return <PendingItem game={game} />;
      })}
      {pendingGames.length > 0 ? <hr className="m-2" /> : null}
      <AddNewGame />
    </div>
  );
}
