import { useEffect, useState } from "react";
import { getCellColorById, getCheckerImageNode, selectNextCell, toField } from "./utils/field-utils";
import { Game, SelectedCells } from "./utils/types";
import ApiRouter from "./utils/router";
import GameInfoColumn from "./game-info-column";
import GameControls from "./game-controls";

let selectedCells: SelectedCells = { first: null, second: null };
let isGameAutoUpdating = false;

export default function GameField(ctx: { game: Game }) {
  const [currentGame, setCurrentGame] = useState(ctx.game);

  useEffect(() => {
    if (!isGameAutoUpdating) {
      isGameAutoUpdating = true;
      ApiRouter.createLongPollingPost<Game>("updated", currentGame, setCurrentGame);
    }
  }, [currentGame]);

  const field = toField(currentGame.board);

  // TODO: break down into components
  return (
    <>
      <div className="flex flex-row justify-center">
        <GameInfoColumn game={currentGame} />
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
      <GameControls game={currentGame} selectedCells={selectedCells} />
    </>
  );
}
