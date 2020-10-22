using Board.Application.Interfaces.Games.Factory;
using Board.Application.Interfaces.Models;
using Board.Application.Interfaces.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Board.Infrastructure.Services
{
    public class GameService: IGameService
    {
        private readonly IGameFactory _gameFactory;

        private ConcurrentDictionary<string, IGame> _games { get; set; } = new ConcurrentDictionary<string, IGame>();


        public GameService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }


        public ICollection<IGame> Games => _games.Values;


        public void AddGame<T>() where T: IGame
        {
            var game = _gameFactory.Create<T>();
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
