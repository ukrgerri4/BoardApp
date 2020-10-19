using Board.Application.Interfaces.Models;
using Board.Application.Interfaces.Services;
using System.Collections.Generic;

namespace Board.Infrastructure.Services
{
    public class GameService: IGameService
    {
        public Dictionary<string, string> UsersInGame { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, IGame> LiveGames { get; set; } = new Dictionary<string, IGame>();

        public void RemovePlayer(string playerId)
        {
            var game = LiveGames.GetValueOrDefault(playerId);
            if (game != null)
            {
                game.RemovePlayer(playerId);
            }

            UsersInGame.Remove(playerId);
        }
    }
}
