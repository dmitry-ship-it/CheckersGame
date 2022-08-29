using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Board
{
    // TODO: Looks like it's redundant. Refactor?
    internal class Cell : ICell
    {
        public static Cell Empty { get; } = new();

        public IChecker? Checker { get; set; }

        public Cell()
        {
            Checker = null;
        }

        public Cell(IChecker checker)
        {
            Checker = checker;
        }
    }
}
