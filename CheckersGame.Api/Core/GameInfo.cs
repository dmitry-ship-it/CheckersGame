using System.Collections.Concurrent;

namespace CheckersGame.Api.Core
{
    public class GamesMap
    {
        private readonly IDictionary<Guid, bool> _games;

        public GamesMap()
        {
            _games = new ConcurrentDictionary<Guid, bool>();
        }

        public int Count => _games.Count;

        public void SetGameStatus(Guid id, bool isPending)
        {
            _games[id] = isPending;
        }

        public bool RemoveIfExist(Guid id)
        {
            return _games.Remove(id);
        }

        public bool IsGamePending(Guid id)
        {
            return _games[id];
        }

        public IEnumerable<KeyValuePair<Guid, bool>> GetAllGamesStatus()
        {
            return _games;
        }

        public bool IsGameExists(Guid id)
        {
            return _games.ContainsKey(id);
        }
    }
}
