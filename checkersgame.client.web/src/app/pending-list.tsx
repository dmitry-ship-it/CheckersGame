import { playerName } from "../header/navbar";
import PendingItem from "./pending-item";
import { useEffect, useState } from "react";
import { startGame } from "../App";
import { PendingGame } from "./utils/types";
import GameTypesDropdown, { SelectedGameType } from "./game-types-dropdown";
import ApiRouter from "./utils/router";

let isGamesAutoUpdating = false;

const addGame = async () => {
  startGame(
    await ApiRouter.post("new", {
      gameType: SelectedGameType,
      playerName: playerName,
    })
  );
  console.log("Game created.");
};

export default function PendingList() {
  // pending games loading and updating
  const [pendingGames, setPendingGames] = useState<PendingGame[]>([]);
  useEffect(() => {
    if (!isGamesAutoUpdating) {
      isGamesAutoUpdating = true;
      // TODO: move to ./utils/router.tsx as long polling post
      const fetchGames = async (games: PendingGame[]) => {
        console.warn("Fetching games... Now games is " + JSON.stringify(games));
        const response = await fetch("https://localhost:7167/api/game/pending", {
          method: "POST",
          headers: {
            "Content-type": "application/json; charset=UTF-8",
          },
          body: JSON.stringify(games),
        });

        if (response.status !== 200) {
          console.warn("Fetch pending games list error");
          await fetchGames(games);
        } else {
          const data = await response.json();
          setPendingGames(data);
          games = data;
        }

        await fetchGames(games);
      };

      fetchGames(pendingGames);
    }
  }, [pendingGames]);

  return (
    <div className="border-2 rounded-xl p-2 max-w-xl mx-auto flex flex-col">
      {pendingGames.map((game) => {
        return <PendingItem gameId={game.gameId} gameType={game.gameType} firstPlayerName={game.firstPlayerName} />;
      })}
      {pendingGames.length > 0 ? <hr className="m-2" /> : null}
      <div className="py-2 px-3 m-2 border rounded-xl hover:border-transparent hover:text-white hover:bg-gradient-to-r from-fuchsia-700 to-cyan-700 flex flex-row items-center">
        <span className="">Add new game: </span>
        <div className="ml-auto">
          <GameTypesDropdown />
          <button className="text-white bg-green-600 font-black w-10 h-10 rounded-xl" onClick={addGame}>
            &#xFF0B;
          </button>
        </div>
      </div>
    </div>
  );
}
