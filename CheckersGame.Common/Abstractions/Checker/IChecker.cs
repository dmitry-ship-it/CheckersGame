using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Abstractions.Checker
{
    public interface IChecker
    {
        IMoveset Moveset { get; init; }

        Color Color { get; init; }
    }
}
