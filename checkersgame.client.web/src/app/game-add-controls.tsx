import { startGame } from "../App";
import { playerName } from "../header/navbar";
import GameTypesDropdown, { SelectedGameType } from "./game-types-dropdown";
import ApiRouter from "./utils/router";

export default function AddNewGame() {
  const addGame = async () => {
    startGame(
      await ApiRouter.post("new", {
        gameType: SelectedGameType,
        playerName: playerName,
      })
    );
    console.log("Game created.");
  };

  return (
    <div className="py-2 px-3 m-2 border rounded-xl hover:border-transparent hover:text-white hover:bg-gradient-to-r from-fuchsia-700 to-cyan-700 flex flex-row items-center">
      <span className="">Add new game: </span>
      <div className="ml-auto">
        <GameTypesDropdown />
        <button className="text-white bg-green-600 font-black w-10 h-10 rounded-xl" onClick={addGame}>
          &#xFF0B;
        </button>
      </div>
    </div>
  );
}
