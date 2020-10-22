using Board.Application.Interfaces.Models;
using System.Collections.Generic;

namespace Board.Application.Interfaces.Services
{
    public interface IGameService
    {
        ICollection<IGame> Games { get; }

        void AddGame(IGame game);
        void RemoveGame(IGame game);
        void RemoveGame(string gameId);
    }
}
