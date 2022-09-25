import { getCellColorById, getCheckerImageNode, selectNextCell } from "./utils/field-utils";
import { SelectedCells } from "./utils/types";

export default function GameFieldCell(ctx: { row: number; col: number; cellDescription: string | null; selectedCells: SelectedCells }) {
  const cellId = ctx.row.toString() + ctx.col.toString();

  return (
    <td
      className={`border-2 border-black h-20 w-20 text-center ${getCellColorById(cellId)}`}
      onClick={(e) => selectNextCell(e, ctx.selectedCells)}
      id={cellId}>
      {getCheckerImageNode(ctx.cellDescription)}
    </td>
  );
}
