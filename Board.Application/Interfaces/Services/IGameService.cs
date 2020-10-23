using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Board.Application.Interfaces.Services
{
    public interface IGameService
    {
        ConcurrentDictionary<string, IGame> Games { get; }

        void AddGame(IGame game);
        void RemoveGame(IGame game);
        void RemoveGame(string gameId);
    }
}
