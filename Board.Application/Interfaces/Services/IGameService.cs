using Board.Application.Interfaces.Models;
using System.Collections.Generic;

namespace Board.Application.Interfaces.Services
{
    public interface IGameService
    {
        Dictionary<string, string> UsersInGame { get; set; }
        Dictionary<string, IGame> LiveGames { get; set; }

        void RemovePlayer(string playerId);
    }
}
