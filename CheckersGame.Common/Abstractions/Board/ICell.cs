using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Abstractions.Board
{
    public interface ICell
    {
        IChecker? Checker { get; set; }
    }
}
