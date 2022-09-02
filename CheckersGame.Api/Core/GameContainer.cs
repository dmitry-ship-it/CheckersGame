using CheckersGame.Common;
using CheckersGame.Common.Abstractions;

namespace CheckersGame.Api.Core
{
    public class GameContainer
    {
        public Guid GameId { get; } = Guid.NewGuid();

        public PlayerInfo FirstPlayer { get; set; }
        public PlayerInfo SecondPlayer { get; set; }

        public IGame Game { get; init; }

        public bool IsEnded => Game.EndMessage is null;

        public GameContainer(IGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            Game = game;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
        }
    }
}
