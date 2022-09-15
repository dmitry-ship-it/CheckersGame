using CheckersGame.Common.Abstrctions;
using CheckersGame.Common.Core;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class BasicMoveset : IMoveset
    {
        private static IMoveset _instance = new BasicMoveset();
        public static IMoveset Instance => _instance ??= new BasicMoveset();

        private BasicMoveset()
        { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, BaseBoard board)
        {
            var invalidResult = ((Cell?)null, false);

            // initial pos is empty
            // or target pos is not empty
            if (board[from] == BaseChecker.Empty
                || board[to] != BaseChecker.Empty)
            {
                return invalidResult;
            }

            // if basic checker can't move both forward and backward;
            // check if player trying to move checker backward 
            if (!board.GameRules.IsBasicCheckerCanMoveBothWays
                &&
                ((board[from]!.DefaultMoveDirection == MoveDirection.FromTopToBottom
                && to.Row - from.Row < 0)
                ||
                (board[from]!.DefaultMoveDirection == MoveDirection.FromBottomToTop
                && to.Row - from.Row > 0)))
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

        private static Cell? GetEnemyPosOnTheWay(Cell from, Cell to, BaseBoard board)
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