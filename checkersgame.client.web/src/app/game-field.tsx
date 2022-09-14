import { useEffect, useState } from "react";
import { getCellColorById, getCheckerImageNode, getStepGameModel, resetSelectedCells, selectNextCell, toField } from "./utils/field-utils";
import { Game, SelectedCells, UpdateGameModel } from "./utils/types";
import ApiRouter from "./utils/router";

let selectedCells: SelectedCells = { first: null, second: null };
let isGameAutoUpdating = false;

export default function GameField(game: Game) {
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
  const formatter = new Intl.NumberFormat("en-US", { minimumIntegerDigits: 2 });

  return (
    <div className="flex flex-row justify-center">
      <div className="flex flex-col justify-around pr-10">
        <div>{game.playerId}</div>
        <div className="justify-center">Me</div>
      </div>
      <div className="flex flex-col">
        <table className="mx-auto">
          {field.map((row, i) => {
            return (
              <tr>
                {row.map((cell, j) => {
                  const cellId = i * field.length + j;
                  return (
                    <td
                      className={`border-2 border-black h-20 w-20 text-center ${getCellColorById(cellId)}`}
                      onClick={(e) => selectNextCell(e, selectedCells)}
                      id={formatter.format(cellId)}>
                      {getCheckerImageNode(cell)}
                    </td>
                  );
                })}
              </tr>
            );
          })}
        </table>
        <button
          className="border-2 border-black bg-green-600 rounded-lg my-4 px-2 py-1 flex place-self-center w-fit"
          onClick={async () => {
            await ApiRouter.post("update", getStepGameModel(currentGame, selectedCells));
            resetSelectedCells(selectedCells);
          }}>
          Send
        </button>
      </div>
    </div>
  );
}
