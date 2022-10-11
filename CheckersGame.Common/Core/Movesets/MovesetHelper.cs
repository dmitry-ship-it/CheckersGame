using CheckersGame.Common.Impl.International.Checker;

namespace CheckersGame.Common.Core.Movesets
{
    public static class MovesetHelper
    {
        public static Cell[] ValidateAndGetEnemies(Cell[] cells, BaseBoard board)
        {
            if (!InitialValidation(cells, board))
            {
                throw new ArgumentException("This move is illegal.", nameof(cells));
            }

            var enemyCells = new List<Cell>();
            for (var i = 1; i < cells.Length; i++)
            {
                var from = cells[i - 1];
                var to = cells[i];

                if (!ValidateSingleMove(from, to, board))
                {
                    throw new ArgumentException("This move is illegal.", nameof(cells));
                }

                // enemy on the way
                var enemyPosition = GetEnemyPosOnTheWay(from, to, board);
                if (enemyPosition is null)
                {
                    continue;
                }

                if (board[from]!.Moveset is BasicMoveset)
                {
                    var stepLength = Math.Abs(to.Col - from.Col);
                    if (stepLength == 2)
                    {
                        enemyCells.Add(enemyPosition.Value);
                    }

                    // single cell step
                    if (stepLength != 1)
                    {
                        throw new ArgumentException("This move is illegal.", nameof(cells));
                    }
                }
                else
                {
                    enemyCells.Add(enemyPosition.Value);
                }
            }

            return enemyCells.ToArray();
        }

        private static Cell? GetEnemyPosOnTheWay(Cell from, Cell to, BaseBoard board)
        {
            return board[from]!.Moveset is BasicMoveset
                ? GetEnemyPosOnTheWayForBasic(from, to, board)
                : GetEnemyPosOnTheWayForStrong(from,to, board);
        }

        private static Cell? GetEnemyPosOnTheWayForBasic(Cell from, Cell to, BaseBoard board)
        {
            var (rowDirection, colDirection) = GetDirections(from, to);

            // step forward by rows and cols
            var row = from.Row + rowDirection;
            var col = from.Col + colDirection;

            var checker = board[row, col];

            if (checker is not null && checker.Color != board[from]?.Color)
            {
                return new Cell { Row = row, Col = col };
            }

            return null;
        }

        private static Cell? GetEnemyPosOnTheWayForStrong(Cell from, Cell to, BaseBoard board)
        {
            var (rowDirection, colDirection) = GetDirections(from, to);

            var enemies = 0;

            Cell? pos = null;

            for (int i = from.Row + rowDirection, j = from.Col + colDirection;
                i != to.Row && j != to.Col;
                i += rowDirection, j += colDirection)
            {
                if (board[i, j] == BaseChecker.Empty)
                {
                    continue;
                }

                if (board[from]!.Color == board[i, j]!.Color)
                {
                    throw new ArgumentException("Can't beat ally.");
                }
                else
                {
                    enemies++;
                    pos ??= new Cell { Row = i, Col = j };
                }
            }

            if (enemies > 1)
            {
                throw new ArgumentException("Can't beat more than one enemy checker.");
            }

            return pos;
        }

        private static (int RowDirection, int ColDirection) GetDirections(Cell from, Cell to)
        {
            var rowDirection = Math.Abs(to.Row - from.Row) / (to.Row - from.Row);
            var colDirection = Math.Abs(to.Col - from.Col) / (to.Col - from.Col);

            return (rowDirection, colDirection);
        }

        private static bool InitialValidation(Cell[] cells, BaseBoard board)
        {
            // need at least two cells
            if (cells.Length < 2)
            {
                return false;
            }

            // initial pos is empty
            // or target pos is not empty
            if (board[cells[0]] == BaseChecker.Empty
                || board[cells[^1]] != BaseChecker.Empty)
            {
                return false;
            }

            return true;
        }

        private static bool ValidateSingleMove(Cell from, Cell to, BaseBoard board)
        {
            var rowDiff = Math.Abs(to.Row - from.Row);
            var colDiff = Math.Abs(to.Col - from.Col);

            // if not diagonal
            if (rowDiff != colDiff)
            {
                return false;
            }

            return CheckMoveDirectionRule(from, to, board);
        }

        // if basic checker can't move both forward and backward;
        // check if player trying to move checker backward 
        private static bool CheckMoveDirectionRule(Cell from, Cell to, BaseBoard board)
        {
            var stepLength = Math.Abs(to.Col - from.Col);

            var canMoveBothWays = board[from]!.Moveset is BasicMoveset
                ? board.GameRules.IsBasicCheckerCanMoveBothWays || stepLength == 2 // any checker can beat both ways
                : board.GameRules.IsStrongCheckerCanMoveBothWays;

            return canMoveBothWays ||
                ((board[from]!.DefaultMoveDirection != MoveDirection.FromTopToBottom || to.Row - from.Row >= 0)
                    &&
                (board[from]!.DefaultMoveDirection != MoveDirection.FromBottomToTop || to.Row - from.Row <= 0));
        }
    }
}
