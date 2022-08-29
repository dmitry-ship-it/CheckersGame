using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Checker
{
    internal class BasicChecker : IChecker
    {
        public IMoveset Moveset { get; init; }

        public Color Color { get; init; }

        public BasicChecker(Color color)
        {
            // Use DI??
            Moveset = new BasicMoveset();
            Color = color;
        }

        // TODO: use more informational result string
        public override string ToString() => "Basic";
    }
}
