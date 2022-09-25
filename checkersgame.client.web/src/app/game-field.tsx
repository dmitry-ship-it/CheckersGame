import { useEffect, useState } from "react";
import { toField } from "./utils/field-utils";
import { Game, SelectedCells } from "./utils/types";
import ApiRouter from "./utils/router";
import GameInfoColumn from "./game-info-column";
import GameControls from "./game-controls";
import GameEndedCard from "./game-ended-card";
import GameFieldCell from "./game-field-cell";

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
                    return <GameFieldCell row={i} col={j} selectedCells={selectedCells} cellDescription={cell} />;
                  })}
                </tr>
              );
            })}
          </table>
        </div>
      </div>
      <GameControls game={currentGame} selectedCells={selectedCells} />
      {currentGame.isEnded && <GameEndedCard />}
    </>
  );
}
