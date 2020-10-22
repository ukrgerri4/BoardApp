using Boadr.Domain.Models.Common.Services;
using Board.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Board.Infrastructure.Services
{
    public class ActiveUserService: IActiveUserService
    {
        private ConcurrentDictionary<string, ActiveUser> _users = new ConcurrentDictionary<string, ActiveUser>();
        
        private Subject<ActiveUser> _userConnectionSubscription = new Subject<ActiveUser>();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ActiveUserService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IObservable<ActiveUser> OnChange()
        {
            return _userConnectionSubscription.AsObservable();
        }

        public async Task Connected(string userId, string connectionId)
        {
            var user = await GetUser(userId);

            _users.AddOrUpdate(
                userId, 
                new ActiveUser(userId, user.UserName, new string[] { connectionId }),
                (key, oldValue) =>
                {
                    oldValue.AddConnection(connectionId);
                    return oldValue;
                }
            );

            _userConnectionSubscription.OnNext(_users[userId]);
        }

        public void Disconnected(string userId, string connectionId)
        {
            if (_users.ContainsKey(userId))
            {
                _users[userId].RemoveConnection(connectionId);
                if (!_users[userId].Connected)
                {
                    _users[userId].StartOffline = DateTime.UtcNow;
                }
            }
        }

        public bool HasGameConnections(string userId) => _users.ContainsKey(userId) && _users[userId].Games.Any();

        public void AddGameConnection(string userId, string gameId)
        {
            if (_users.ContainsKey(userId))
            {
                _users[userId].AddGame(gameId);
            }
        }

        public void RemoveGameConnection(string userId, string gameId)
        {
            if (_users.ContainsKey(userId))
            {
                _users[userId].RemoveGame(gameId);
            }
        }

        private async Task<IdentityUser> GetUser(string userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                return await userManager.FindByIdAsync(userId);
            }
        }
    }
}
