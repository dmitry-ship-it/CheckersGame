using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;

namespace CheckersGame.Api.Core
{
    // TODO: Rename with 'GameManager'. Also rename all variable names of this type.
    public class GameContainer
    {
        private readonly IGame _game;

        private static readonly SortedDictionary<Guid, bool> _pendingGames = new();

        public GameContainer(IGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            _game = game;
            Players = (firstPlayer, secondPlayer);

            CurrentPlayerTurn = Players.First;

            PushTurnToNextPlayer();
        }

        public Guid GameId { get; } = Guid.NewGuid();

        public PlayerInfo CurrentPlayerTurn { get; private set; }

        public (PlayerInfo First, PlayerInfo Second) Players { get; set; }

        public IBoard Board => _game.Board;

        public bool IsEnded => _game.EndMessage is null;

        // TODO: Replace implementation with GameType enum name
        public string GameType =>
            _game.GetType().Name.Replace("Game", string.Empty);

        public void NextTurn(Cell from, Cell to)
        {
            _game.NextTurn(from, to);
            PushTurnToNextPlayer();
        }

        public PlayerInfo GetEnemyForPlayer(Guid enemyPlayerId)
        {
            if (Players.First.Id == enemyPlayerId)
            {
                return Players.Second;
            }
            else if (Players.Second.Id == enemyPlayerId)
            {
                return Players.First;
            }
            else
            {
                throw new ArgumentException("There is no player with this id.", nameof(enemyPlayerId));
            }
        }

        // TODO: Merge this two methods --^ --v
        public PlayerInfo GetPlayerById(Guid id)
        {
            if (Players.First.Id == id)
            {
                return Players.First;
            }
            else if (Players.Second.Id == id)
            {
                return Players.Second;
            }
            else
            {
                throw new ArgumentException("There is no player with this id.", nameof(id));
            }
        }

        public static void SetGameStatus(Guid id, bool isPending)
        {
            lock (_pendingGames)
            {
                _pendingGames[id] = isPending;
            }
        }

        public static bool RemoveGame(Guid id)
        {
            lock (_pendingGames)
            {
                return _pendingGames.Remove(id);
            }
        }

        public static bool GetGameStatus(Guid id)
        {
            return _pendingGames[id];
        }

        public static int GetGamesCount()
        {
            return _pendingGames.Count;
        }

        public static IEnumerable<KeyValuePair<Guid, bool>> GetAllGamesStatus()
        {
            // create copy to avoid concurrency
            return _pendingGames.ToArray();
        }

        public static bool IsGameExists(Guid id)
        {
            return _pendingGames.ContainsKey(id);
        }

        private void PushTurnToNextPlayer()
        {
            CurrentPlayerTurn = _game.Players.First == _game.CurrentPlayerTurn
                ? Players.First
                : Players.Second;
        }
    }
}