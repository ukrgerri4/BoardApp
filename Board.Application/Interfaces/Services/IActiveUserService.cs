using Board.Application.Models;
using System;
using System.Threading.Tasks;

namespace Board.Application.Interfaces.Services
{
    public interface IActiveUserService
    {
        IObservable<ActiveUser> OnChange();
        
        Task Connected(string userId, string connectionId);
        void Disconnected(string userId, string connectionId);
        
        bool HasGameConnections(string userId);
        void AddGameConnection(string userId, string gameId);
        void RemoveGameConnection(string userId, string gameId);
    }
}
