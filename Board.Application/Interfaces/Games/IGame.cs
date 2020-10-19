using System.Collections.Generic;

namespace Board.Application.Interfaces.Models
{
    public interface IGame
    {
        string Id { get; set; }
        List<string> PlayerIds { get; set; }

        void End();
        void Start();
        void Update();

        void RemovePlayer(string playerId);
        void AddPlayer(string playerId);
    }
}
