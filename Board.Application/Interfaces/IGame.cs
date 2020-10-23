using Microsoft.AspNetCore.Identity;

namespace Board.Application.Interfaces
{
    public interface IGame
    { 
        string Id { get; set; }
        string Name { get; set; }

        void Start();
        void Pause();
        void End();
        void Update();

        void RemovePlayer(string playerId);
        void AddPlayer(IdentityUser user);
        object GameStatePacket();
    }
}
