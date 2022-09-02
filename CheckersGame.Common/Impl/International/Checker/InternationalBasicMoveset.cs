using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Impl.General;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class InternationalBasicMoveset : IMoveset
    {
        private static IMoveset _instance = new InternationalBasicMoveset();
        public static IMoveset Instance => _instance ??= new InternationalBasicMoveset();

        private InternationalBasicMoveset()
        { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board)
        {
            var invalidResult = ((Cell?)null, false);

            // initial pos is empty
            // or target pos is not empty
            if (board[from] == BaseChecker.Empty
                || board[to] != BaseChecker.Empty)
            {
                return invalidResult;
            }

            var rowDiff = Math.Abs(to.Row - from.Row);
            var colDiff = Math.Abs(to.Col - from.Col);

            // if not diagonal
            if (rowDiff != colDiff)
            {
                return invalidResult;
            }

            // enemy on the way
            var enemy = GetEnemyPosOnTheWay(from, to, board);
            if (rowDiff == 2 && enemy is not null)
            {
                return (enemy, true);
            }

            // single cell step
            return (null, rowDiff == 1);
        }

        private static Cell? GetEnemyPosOnTheWay(Cell from, Cell to, IBoard board)
        {
            var rowDirection = Math.Abs(to.Row - from.Row) / (to.Row - from.Row);
            var colDirection = Math.Abs(to.Col - from.Col) / (to.Col - from.Col);

            var row = from.Row + rowDirection;
            var col = from.Col + colDirection;

            var checker = board[row, col];

            if (checker is not null && checker.Color != board[from]?.Color)
            {
                return new Cell { Row = row, Col = col };
            }

            return null;
        }

        public override string ToString()
        {
            return "Basic";
        }
    }
}