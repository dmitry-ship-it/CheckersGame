import { Game, SelectedCells, StepGameModel } from "./types";

export const CellDarkBgColor = "bg-neutral-500";
export const CellLightBgColor = "bg-white";

export const CellFirstSelectedBgColor = "bg-orange-500";
export const CellSecondSelectedBgColor = "bg-red-500";

export const getCheckerImageNode = (checkerName: string | null) => {
  if (checkerName === null) {
    return null;
  }

  let path: string | null = null;

  switch (checkerName) {
    case "BasicBlack":
      path = "./assets/basic-black-checker.png";
      break;
    case "BasicWhite":
      path = "./assets/basic-white-checker.png";
      break;
    case "StrongBlack":
      path = "./assets/strong-black-checker.png";
      break;
    case "StrongWhite":
      path = "./assets/strong-white-checker.png";
      break;
  }

  return path !== null ? <img className="h-3/4 w-3/4 m-auto" src={path} alt={checkerName} /> : <div>{checkerName}</div>;
};

export const getCellColorById = (id: string): string => {
  const row = Number.parseInt(id[0]);
  const col = Number.parseInt(id[1]);
  return (row + col) % 2 === 1 ? CellDarkBgColor : CellLightBgColor;
};

export const clearBgColor = (element: Element) => {
  element.classList.forEach((className) => {
    if (className.startsWith("bg-")) {
      element.classList.remove(className);
    }
  });
};

export const toField = (board: (string | null)[]) => {
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
};

export const resetSelectedCells = (selectedCells: SelectedCells) => {
  if (selectedCells.first === null || selectedCells.second === null) return;

  clearBgColor(selectedCells.first);
  selectedCells.first.classList.add(getCellColorById(selectedCells.first.id));
  selectedCells.first = null;

  clearBgColor(selectedCells.second);
  selectedCells.second.classList.add(getCellColorById(selectedCells.second.id));
  selectedCells.second = null;
};

export const getStepGameModel = (game: Game, selectedCells: SelectedCells): StepGameModel => {
  if (selectedCells.first === null || selectedCells.second === null) {
    throw Error("Cells are not selected.");
  }
  return {
    gameId: game.id,
    playerId: game.playerId,
    from: {
      row: Number.parseInt(selectedCells.first.id[0]),
      col: Number.parseInt(selectedCells.first.id[1]),
    },
    to: {
      row: Number.parseInt(selectedCells.second.id[0]),
      col: Number.parseInt(selectedCells.second.id[1]),
    },
  };
};

export const selectNextCell = (event: React.MouseEvent, selectedCells: SelectedCells) => {
  if (selectedCells.first == null) {
    selectedCells.first = event.currentTarget;
    clearBgColor(selectedCells.first);
    event.currentTarget.classList.add(CellFirstSelectedBgColor);
  } else if (selectedCells.second == null) {
    selectedCells.second = event.currentTarget;
    clearBgColor(selectedCells.second);
    event.currentTarget.classList.add(CellSecondSelectedBgColor);
  } else {
    resetSelectedCells(selectedCells);
  }
  console.log(selectedCells);
};
