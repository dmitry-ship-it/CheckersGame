using CheckersGame.Api.Core;

namespace CheckersGame.Api.Models
{
    public class GameModel : IEquatable<GameContainer>
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public string FirstPlayerName { get; set; } = default!;
        public string SecondPlayerName { get; set; } = default!;

        public IEnumerable<string?> Board { get; set; } = default!;

        public string CurrentPlayerTurn { get; set; } = default!;

        public bool IsEnded { get; set; }

        public bool Equals(GameContainer? other)
        {
            if (other is null)
            {
                return false;
            }

            if (GetHashCode() == other.GetHashCode())
            {
                return true;
            }

            if (FirstPlayerName != other.Players.First.Name
                || SecondPlayerName != other.Players.Second.Name)
            {
                return false;
            }

            return Board.SequenceEqual(other.Board);
        }

        public static bool operator ==(GameModel lhs, GameContainer rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator ==(GameContainer lhs, GameModel rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GameModel lhs, GameContainer rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator !=(GameContainer lhs, GameModel rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as GameContainer);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstPlayerName, SecondPlayerName, Board);
        }
    }
}
