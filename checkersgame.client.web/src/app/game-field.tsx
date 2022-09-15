import { useEffect, useState } from "react";
import { getCellColorById, getCheckerImageNode, getStepGameModel, resetSelectedCells, selectNextCell, toField } from "./utils/field-utils";
import { Game, SelectedCells, UpdateGameModel } from "./utils/types";
import ApiRouter from "./utils/router";

let selectedCells: SelectedCells = { first: null, second: null };
let isGameAutoUpdating = false;

export default function GameField(game: Game) {
  const [error, setError] = useState("");
  const [currentGame, setCurrentGame] = useState(game);

  useEffect(() => {
    if (!isGameAutoUpdating) {
      isGameAutoUpdating = true;
      const updateGame = async (gameState: Game) => {
        console.warn("Fetching game update...");
        const gameInfo: UpdateGameModel = {
          gameId: gameState.id,
          playerId: gameState.playerId,
          board: gameState.board,
        };
        const response = await fetch("https://localhost:7167/api/game/updated", {
          method: "POST",
          body: JSON.stringify(gameInfo),
          headers: {
            "Content-type": "application/json; charset=UTF-8",
          },
        });
        if (response.status !== 200) {
          console.warn(`Fetching game error (${response.status}). Continuing...`);
          await updateGame(gameState);
        } else {
          console.log("Game update fetched!");
          const data: Game = await response.json();
          gameState = data;
          setCurrentGame(data);
        }
        await updateGame(gameState);
      };
      updateGame(currentGame);
    }
  }, [currentGame]);

  const field = toField(currentGame.board);

  // TODO: break down into components
  return (
    <>
      <div className="flex flex-row justify-center">
        <div className="flex flex-col justify-around max-w-xl pr-10">
          <div className="px-4 py-2 rounded-xl bg-slate-400">
            <div className="">Username:</div>
            <div className="inline-block overflow-hidden overflow-ellipsis w-32 font-bold">{game.secondPlayerName}</div>
          </div>
          <div className="px-4 py-2 rounded-xl bg-slate-400">
            <div className="">Username:</div>
            <div className="inline-block overflow-hidden overflow-ellipsis w-32 font-bold">{game.firstPlayerName}</div>
          </div>
        </div>
        <div className="flex flex-col">
          <table className="mx-auto">
            {field.map((row, i) => {
              return (
                <tr>
                  {row.map((cell, j) => {
                    const cellId = i.toString() + j.toString();
                    return (
                      <td
                        className={`border-2 border-black h-20 w-20 text-center ${getCellColorById(cellId)}`}
                        onClick={(e) => selectNextCell(e, selectedCells)}
                        id={cellId}>
                        {getCheckerImageNode(cell)}
                      </td>
                    );
                  })}
                </tr>
              );
            })}
          </table>
        </div>
      </div>
      <div className="flex flex-row justify-center">
        <div className="flex flex-col justify-center">
          <button
            className="border-2 border-black bg-green-600 rounded-lg my-4 px-2 py-1 flex place-self-center w-fit"
            onClick={async () => {
              try {
                await ApiRouter.post("update", getStepGameModel(currentGame, selectedCells));
              } catch (e) {
                setError((e as Error).message);
                setTimeout(() => {
                  setError("");
                }, 5000);
              }
              resetSelectedCells(selectedCells);
            }}>
            Send
          </button>
          <div className="h-8 text-red-600 place-self-center">
            {error.length !== 0 ? <div className="border-2 border-pink-700 p-3">Error: {error}</div> : null}
          </div>
        </div>
      </div>
    </>
  );
}
