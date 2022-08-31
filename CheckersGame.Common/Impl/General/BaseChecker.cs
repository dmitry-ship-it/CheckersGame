using CheckersGame.Common.Abstractions.Checker;
using System.Drawing;

namespace CheckersGame.Common.Impl.General
{
    internal class BaseChecker : IChecker
    {
        public BaseChecker(Color color, IMoveset moveset)
        {
            Color = color;
            Moveset = moveset;
        }

        public IMoveset Moveset { get; set; }

        public Color Color { get; init; }

        public static BaseChecker Empty { get; } = null!;

        public object Clone()
        {
            return new BaseChecker(Color, Moveset);
        }
    }
}