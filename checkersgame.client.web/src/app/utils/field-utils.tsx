import { Cell, Game, SelectedCells, StepGameModel } from "./types";

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
  if (selectedCells.cells.length === 0) return;

  for (const cell of selectedCells.cells) {
    clearBgColor(cell);
    cell.classList.add(getCellColorById(cell.id));
  }

  selectedCells.cells = [];
};

export const getStepGameModel = (game: Game, selectedCells: SelectedCells): StepGameModel => {
  if (selectedCells.cells.length < 2) {
    throw Error("You have to select at least two cells.");
  }
  return {
    gameId: game.id,
    playerId: game.playerId,
    cells: selectedCells.cells.map<Cell>((el) => ({
      row: Number.parseInt(el.id[0]),
      col: Number.parseInt(el.id[1]),
    })),
  };
};

export const selectNextCell = (event: React.MouseEvent, selectedCells: SelectedCells) => {
  if (selectedCells.cells.includes(event.currentTarget)) return;

  // fill bg with numbers??
  clearBgColor(event.currentTarget);
  event.currentTarget.classList.add("bg-red-700");
  selectedCells.cells.push(event.currentTarget);

  console.log(selectedCells);
};
