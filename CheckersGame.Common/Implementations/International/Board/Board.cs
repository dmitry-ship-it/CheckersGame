using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Implementations.International.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Board
{
    internal class Board : IBoard
    {
        public int Rows { get; init; }

        public int Cols { get; init; }

        private readonly ICell[,] _cells;

        public ICell this[int row, int col]
        {
            get => _cells[row, col];
            set => _cells[row, col] = value;
        }

        public Board()
        {
            Rows = 10;
            Cols = 10;
            _cells = ConstructBoard(Rows, Cols);
        }

        // TODO: Refactor
        public void MoveChecker(
            (int row, int col) from,
            (int row, int col) to,
            Color playerColor)
        {
            if (_cells[from.row, from.col]?.Checker is null)
            {
                throw new ArgumentException("This cell is empty");
            }

            if (!_cells[from.row, from.col].Checker!.Moveset.IsLegalMove(from, to, this))
            {
                throw new ArgumentException("This move is illegal");
            }

            // check if there was enemy on the way
            var enemyPos = _cells[from.row, from.col].Checker!.Moveset.TryGetEnemyPosOnTheWay(from, to, this);

            if (enemyPos is not null)
            {
                _cells[enemyPos.Value.row, enemyPos.Value.col] = Cell.Empty;
            }

            // swap
            (_cells[to.row, to.col],
             _cells[from.row, from.col])
                =
            (_cells[from.row, from.col],
             _cells[to.row, to.col]);
        }

        // TODO: Refactor
        private ICell[,] ConstructBoard(int rows, int cols)
        {
            ICell[,] cells = new Cell[rows, cols];
            var factory = new CheckerFactory();

            FillRows(cells, 0, 4, new Cell()
            {
                Checker = factory.CreateNewBlack(CheckerType.Basic)
            });

            FillRows(cells, 4, 2, Cell.Empty);

            FillRows(cells, 6, 4, new Cell()
            {
                Checker = factory.CreateNewWhite(CheckerType.Basic)
            });

            return cells;
        }

        private void FillRows(ICell[,] cells, int start, int count, Cell cell)
        {
            for (var i = start; i < start + count; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        cells[i, j] = cell;
                    }
                }
            }
        }
    }
}
