using Microsoft.Extensions.Caching.Memory;

namespace CheckersGame.Api.Core
{
    public class GamesVault
    {
        private readonly IMemoryCache _cache;
        private readonly GameContainerFactory _gameFactory;

        public GamesVault(IMemoryCache cache, GameContainerFactory gameFactory)
        {
            _cache = cache;
            _gameFactory = gameFactory;
        }

        public GamesMap Vault { get; } = new();

        public GameContainer? Get(Guid gameId)
        {
            return _cache.Get<GameContainer>(gameId);
        }

        public GameContainer Create(GameType gameType, string firstPlayerName)
        {
            var game = _gameFactory.Create(gameType, firstPlayerName);

            var cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(5)); // remove game from dictionary on expiration
            cacheOptions.RegisterPostEvictionCallback((key, _, _, _) => Vault.RemoveIfExist((Guid)key));
            _cache.Set(game.GameId, game, cacheOptions);

            return game;
        }
    }
}
