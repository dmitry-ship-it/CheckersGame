using CheckersGame.Common.Abstrctions;
using System.Drawing;

namespace CheckersGame.Common.Core
{
    public class BaseChecker
    {
        public BaseChecker(Color color, IMoveset moveset, MoveDirection defaultMoveDirection)
        {
            Color = color;
            Moveset = moveset;
            DefaultMoveDirection = defaultMoveDirection;
        }

        public readonly MoveDirection DefaultMoveDirection;

        public IMoveset Moveset { get; set; }

        public Color Color { get; init; }

        public static BaseChecker? Empty { get; }

        public object Clone()
        {
            return new BaseChecker(Color, Moveset, DefaultMoveDirection);
        }

        public override string ToString()
        {
            return $"{Moveset}{Color.Name}";
        }
    }
}