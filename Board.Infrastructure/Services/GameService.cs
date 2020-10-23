using Board.Application.Interfaces;
using Board.Application.Interfaces.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Board.Infrastructure.Services
{
    public class GameService: IGameService
    {
        private ConcurrentDictionary<string, IGame> _games { get; set; } = new ConcurrentDictionary<string, IGame>();


        public ConcurrentDictionary<string, IGame> Games => _games;


        public void AddGame(IGame game)
        {
            _games.TryAdd(game.Id, game);
        }

        public void RemoveGame(IGame game)
        {
            RemoveGame(game.Id);
        }

        public void RemoveGame(string gameId)
        {
            _games.TryRemove(gameId, out _);
        }
    }
}
