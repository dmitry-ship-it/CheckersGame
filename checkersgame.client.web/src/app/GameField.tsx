import { useState } from "react";
import { CurrentGame } from "./pending-list";

interface Selected {
  first: Element | null;
  second: Element | null;
}

interface UpdateGameModel {
  gameId: string;
  playerId: string;
  from: {
    row: number,
    col: number
  },
  to: {
    row: number,
    col: number
  }
}

let selected: Selected = { first: null, second: null };

let setFirstSelectedCallback : any;
let setSecondSelectedCallback : any;
let setCurrentGameCallback: any;

const cellDarkBgColor = "bg-neutral-500";
const cellLightBgColor = "bg-white";

const cellFirstSelectedBgColor = "bg-orange-500";
const cellSecondSelectedBgColor = "bg-red-500";

const sendStep = async (game: CurrentGame) => {

  if (selected.first === null || selected.second === null) {
    throw Error("Cells are not selected.");
  }

  let step: UpdateGameModel = {
    gameId: game.id,
    playerId: game.playerId,
    from: {
      row: Number.parseInt(selected.first.id[0]),
      col: Number.parseInt(selected.first.id[1]),
    },
    to: {
      row: Number.parseInt(selected.second.id[0]),
      col: Number.parseInt(selected.second.id[1]),
    }
  }

  const response = await fetch("https://localhost:7167/api/game/update", {
    method: "POST",
    body: JSON.stringify(step),
    headers: {
      "Content-type": "application/json; charset=UTF-8",
    }
  });

  const data = await response.json();

  resetSelectedObj();
  setCurrentGameCallback(data);
}

const getField = (board: (string | null)[]) => {
  const side = Math.sqrt(board.length);

  let field: (string | null)[][] = new Array(side).fill(false).map(() => new Array(side).fill(false));

  let k = 0;
  for (let i = 0; i < side; i++) {
    for (let j = 0; j < side; j++) {
      field[i][j] = board[k];
      k++;
    }
  }

  return field;
}

const clearBgColor = (element: Element) => {
  element.classList.forEach(className => {
    if (className.startsWith("bg-")) {
      element.classList.remove(className);
    }
  });
}

const getCellColorByIdStr = (id: string) => {
  return getCellColorByIdNum(Number.parseInt(id));
};

const getCellColorByIdNum = (id: number) => {
  return (Math.floor(id / 10) + id % 10) % 2 === 1
    ? cellDarkBgColor
    : cellLightBgColor;
};

const getCheckerImage = (description: string | null) => {
  if (description === null) {
    return null;
  }

  let path: string | null = null;

  switch (description) {
    case "BasicBlack":
      path = "./basic-black-checker.png";
      break;
    case "BasicWhite":
      path = "./basic-white-checker.png";
      break;
    case "StrongBlack":
      path = "./strong-black-checker.png";
      break;
    case "StrongWhite":
      path = "./strong-white-checker.png";
      break;
  }

  return path !== null 
    ? <img className="h-3/4 w-3/4 m-auto" src={path} alt={description} />
    : <div>{description}</div>
}

const resetSelectedObj = () => {

  if (selected.first === null || selected.second === null) return;

  clearBgColor(selected.first);
  selected.first.classList.add(getCellColorByIdStr(selected.first.id));
  selected.first = null;

  clearBgColor(selected.second);
  selected.second.classList.add(getCellColorByIdStr(selected.second.id));
  selected.second = null;

  setFirstSelectedCallback(false);
  setSecondSelectedCallback(false);
}

const selectCell = (event : React.MouseEvent) => {

  if (selected.first == null) {
    selected.first = event.currentTarget;
    clearBgColor(selected.first);
    event.currentTarget.classList.add(cellFirstSelectedBgColor);
    setFirstSelectedCallback(true);
  } else if (selected.second == null) {
    selected.second = event.currentTarget;
    clearBgColor(selected.second);
    event.currentTarget.classList.add(cellSecondSelectedBgColor);
    setSecondSelectedCallback(true);
  } else {
    resetSelectedObj();
  }

  console.log(selected);
};

export default function GameField(game: CurrentGame) {

  const [isFirstSelected, setIsFirstSelected] = useState(false);
  const [isSecondSelected, setIsSecondSelected] = useState(false);
  const [currentGame, setCurrentGame] = useState(game);

  setFirstSelectedCallback = setIsFirstSelected;
  setSecondSelectedCallback = setIsSecondSelected;
  setCurrentGameCallback = setCurrentGame;

  const field = getField(currentGame.board);
  const formatter = new Intl.NumberFormat('en-US', {minimumIntegerDigits: 2});

  return (
    <div className="mx-auto flex flex-wrap flex-col">
      <table className="mx-auto">
        {field.map((row, i) => {
          return (
            <tr>
              {row.map((cell, j) => {
                const cellId = i * field.length + j;
                return <td className={"border-2 border-black h-20 w-20 text-center " 
                  + getCellColorByIdNum(cellId)}
                  onClick={(e) => selectCell(e)}
                  id={formatter.format(cellId)}>{getCheckerImage(cell)}</td>
              })}
            </tr>
          );
        })}
      </table>
      <button className="border-2 border-black bg-green-600 rounded-lg my-4 px-2 py-1 flex place-self-center w-fit"
              onClick={() => sendStep(currentGame)}>Send</button>
    </div>
  );
}
