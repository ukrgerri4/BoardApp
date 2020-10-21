using Board.Application.Enumerations;
using System.Collections.Generic;

namespace Board.Application.Interfaces.Models
{
    public interface IGame
    { 
        string Id { get; set; }
        string Name { get; set; }
        GameState State { get; set; }

        void Start();
        void Pause();
        void End();
        void Update();

        void RemovePlayer(string playerId);
        void AddPlayer(string playerId);
    }
}
