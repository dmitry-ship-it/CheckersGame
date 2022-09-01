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

        public static IChecker? Empty { get; }

        public object Clone()
        {
            return new BaseChecker(Color, Moveset);
        }

        public override string ToString()
        {
            return $"{Moveset}{Color.Name}";
        }
    }
}