import { useState } from "react";
import ErrorCard from "./error-card";
import { getStepGameModel, resetSelectedCells } from "./utils/field-utils";
import ApiRouter from "./utils/router";
import { Game, SelectedCells } from "./utils/types";

export default function GameControls(ctx: { game: Game; selectedCells: SelectedCells }) {
  const [error, setError] = useState("");

  return (
    <div className="flex flex-row justify-center">
      <div className="flex flex-col justify-center">
        <button
          className="border-2 border-black bg-green-600 rounded-lg my-4 px-2 py-1 flex place-self-center w-fit"
          onClick={async () => {
            try {
              await ApiRouter.post("update", getStepGameModel(ctx.game, ctx.selectedCells));
            } catch (e) {
              setError((e as Error).message);
              setTimeout(() => {
                setError("");
              }, 5000);
            }
            resetSelectedCells(ctx.selectedCells);
          }}>
          Send
        </button>
        <ErrorCard message={error} />
      </div>
    </div>
  );
}
