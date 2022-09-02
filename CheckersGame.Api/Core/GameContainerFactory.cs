using CheckersGame.Common;

namespace CheckersGame.Api.Core
{
    public class GameContainerFactory
    {
        private readonly GameFactory _gameFactory;

        public GameContainerFactory(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public GameContainer Create(
            GameType gameType,
            string? firstPlayerName = null,
            string? secondPlayerName = null)
        {
            if (string.IsNullOrEmpty(firstPlayerName))
            {
                firstPlayerName = "Noname";
            }

            if (string.IsNullOrEmpty(secondPlayerName))
            {
                secondPlayerName = "Noname";
            }

            var first = new PlayerInfo(firstPlayerName);
            var second = new PlayerInfo(secondPlayerName);

            return new GameContainer(_gameFactory.CreateNew(gameType), first, second);
        }
    }
}
