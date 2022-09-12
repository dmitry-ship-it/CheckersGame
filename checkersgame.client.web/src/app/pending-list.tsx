import { playerName } from "../header/navbar";
import PendingItem from "./pending-item";
import { useEffect, useState } from "react";
import { startGame } from "../App";

export interface PendingGame {
  gameId: string;
  gameType: string;
  firstPlayerName: string;
}

export interface NewGameRequestModel {
  gameType: string;
  playerName: string;
}

export interface CurrentGame {
  id: string;
  playerId: string;
  board: (string | null)[];
  currentPlayerTurn: string;
  isEnded: boolean;
}

let gameRequestBody: NewGameRequestModel = {
  gameType: "",
  playerName: "",
};

let isGamesAutoloading = false;

const addGame = async () => {
  gameRequestBody.playerName = playerName;

  const response = await fetch("https://localhost:7167/api/game/new", {
    method: "POST",
    body: JSON.stringify(gameRequestBody),
    headers: {
      "Content-type": "application/json; charset=UTF-8",
    },
  });

  startGame(await response.json());

  console.log("Created.");
};

export default function PendingList() {
  // game types loading and updating
  const [gameTypes, setGameTypes] = useState<string[]>([]);
  const fetchTypes = async () => {
    console.warn("Fetching game types...");
    const response = await fetch("https://localhost:7167/api/game/types");
    const data = await response.json();
    setGameTypes(data);
    gameRequestBody.gameType = data.at(0);
  };
  useEffect(() => {
    fetchTypes();
  }, []);

  // pending games loading and updating
  const [pendingGames, setPendingGames] = useState<PendingGame[]>([]);
  useEffect(() => {
    if (!isGamesAutoloading) {
      isGamesAutoloading = true;
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
    <div className="border-2 p-2 w-fit mx-auto">
      {pendingGames.map((game) => {
        return <PendingItem gameId={game.gameId} gameType={game.gameType} firstPlayerName={game.firstPlayerName} />;
      })}
      <div className="px-3">
        <span className="">Add new game: </span>
        <select
          className="mx-3 py-2 px-1 rounded-lg"
          onChange={(e) => {
            gameRequestBody.gameType = e.currentTarget.value;
            console.log(gameRequestBody.gameType);
          }}>
          {gameTypes.map((gameType) => {
            return <option value={gameType}>{gameType}</option>;
          })}
        </select>
        <button className="text-white bg-green-600 font-black w-10 h-10 rounded-lg" onClick={addGame}>
          &#xFF0B;
        </button>
      </div>
    </div>
  );
}
