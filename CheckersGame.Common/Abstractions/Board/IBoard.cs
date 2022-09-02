using CheckersGame.Common.Abstractions.Checker;

namespace CheckersGame.Common.Abstractions.Board
{
    public interface IBoard : IEnumerable<string?>
    {
        int Rows { get; init; }
        int Cols { get; init; }

        IChecker? this[int row, int col] { get; set; }

        IChecker? this[Cell cell] { get; set; }

        void HandleMove(Cell from, Cell to, out bool isCheckerBeaten);
    }
}