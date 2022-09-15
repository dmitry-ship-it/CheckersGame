using CheckersGame.Api.Core;
using CheckersGame.Common.Impl.International.Checker;
using System.Collections;
using System.Drawing;

namespace CheckersGame.Common.Core
{
    public class BaseBoard : IEnumerable<string?>
    {
        private readonly BaseChecker?[,] _board;

        public BaseBoard(GameType gameType, GameRules gameRules)
        {
            _board = GenerateBoard(gameRules.FieldSize);
            GameType = gameType;
            GameRules = gameRules;
            Rows = gameRules.FieldSize;
            Cols = gameRules.FieldSize;
        }

        public BaseChecker? this[Cell cell]
        {
            get => this[cell.Row, cell.Col];
            set => this[cell.Row, cell.Col] = value;
        }

        public BaseChecker? this[int row, int col]
        {
            get => _board[row, col];
            set => _board[row, col] = value;
        }

        public int Rows { get; init; }
        public int Cols { get; init; }

        public GameType GameType { get; init; }
        public GameRules GameRules { get; init; }

        private static BaseChecker?[,] GenerateBoard(int fieldSize)
        {
            var board = new BaseChecker[fieldSize, fieldSize];

            // count of rows of single player to fill with checkers
            var rowsToFill = (fieldSize - 2) / 2;

            // fill rows for second player
            var startRow = 0;
            FillRows(board, startRow, rowsToFill, new BaseChecker(Color.Black, BasicMoveset.Instance, MoveDirection.FromTopToBottom));
            startRow += rowsToFill;

            // skip 2 rows
            FillRows(board, startRow, 2, BaseChecker.Empty);
            startRow += 2;

            FillRows(board, startRow, rowsToFill, new BaseChecker(Color.White, BasicMoveset.Instance, MoveDirection.FromBottomToTop));

            return board;
        }

        private static void FillRows(BaseChecker?[,] board, int start, int count, BaseChecker? checker)
        {
            for (var i = start; i < start + count; i++)
            {
                for (var j = 0; j < board.GetLength(1); j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        board[i, j] = checker is null ? null : (BaseChecker)checker.Clone();
                    }
                }
            }
        }

        public void HandleMove(Cell from, Cell to, out bool isCheckerBeaten)
        {
            if (this[from] == BaseChecker.Empty)
            {
                throw new ArgumentException("This cell is empty", nameof(from));
            }

            var (enemyCell, isLegalMove) = this[from]!.Moveset.GetInfo(from, to, this);

            if (!isLegalMove)
            {
                throw new InvalidOperationException("This move is illegal");
            }

            if (enemyCell is not null)
            {
                this[enemyCell.Value] = BaseChecker.Empty;
            }

            isCheckerBeaten = enemyCell.HasValue;

            TryUpgradeChecker(from, to);
            SwapCheckers(from, to);
        }

        private void TryUpgradeChecker(Cell from, Cell to)
        {
            if (this[from]!.Color == Color.White && to.Row == 0)
            {
                this[from]!.Moveset = StrongMoveset.Instance;
            }
            else if (this[from]!.Color == Color.Black && to.Row == Rows - 1)
            {
                this[from]!.Moveset = StrongMoveset.Instance;
            }
        }

        private void SwapCheckers(Cell from, Cell to)
        {
            (this[to], this[from]) = (this[from], this[to]);
        }

        public IEnumerator<string?> GetEnumerator()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    yield return _board[i, j]?.ToString();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}