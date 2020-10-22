using Boadr.Domain.Enumerations;
using Boadr.Domain.Models.Mafia;
using Board.Application.Interfaces.Models;
using Board.Application.Interfaces.Services;
using Board.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Board.Infrastructure.Games
{
    public class MafiaGame : IGame
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IActiveUserService _activeUserService;
        public string Id { get; set; }
        public string Name { get; set; }
        public GameState State { get; set; }


        public int MaxPlayers { get; set; }
        private Dictionary<string, Player> ConnectedPlayerIds { get; set; }
        
        private Subject<Unit> _notifyer = new Subject<Unit>();
        private Subject<Unit> _destroy = new Subject<Unit>();


        public MafiaGame(IServiceScopeFactory serviceScopeFactory, IActiveUserService activeUserService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _activeUserService = activeUserService;

            Id = Guid.NewGuid().ToString();
            Name = $"Game-{Id}";
            State = GameState.Created;
            ConnectedPlayerIds = new Dictionary<string, Player>();

            _activeUserService.OnChange()
                .TakeUntil(_destroy)
                .Where(userInfo => ConnectedPlayerIds.ContainsKey(userInfo.UserId))
                .Do(_ => Console.WriteLine($"Game - {Id}. Find user - {_.UserId} - {_.Connected}"))
                .Subscribe(userInfo =>
                {
                    ConnectedPlayerIds[userInfo.UserId].Connected = userInfo.Connected;
                    Update();
                });

            _notifyer.AsObservable()
                .TakeUntil(_destroy)
                .Sample(TimeSpan.FromMilliseconds(1000))
                .Where(_ => State != GameState.Ended)
                .Do(_ => Console.WriteLine($"Game - {Id}"))
                .Subscribe(_ => Notify());
        }

        public void Start()
        {
            State = GameState.Started;
        }

        public void Pause()
        {
            State = GameState.Paused;
        }

        public void End()
        {
            State = GameState.Ended;
            _destroy.OnNext(Unit.Default);
            _destroy.OnCompleted();
            _notifyer.OnCompleted();
        }

        public void Update()
        {
            _notifyer.OnNext(Unit.Default);
        }

        public void AddPlayer(string userId)
        {
            ConnectedPlayerIds.TryAdd(userId, new Player());
            _activeUserService.AddGameConnection(userId, Id);

        }

        public void RemovePlayer(string playerId)
        {
            ConnectedPlayerIds.Remove(playerId);
        }

        private void Notify()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MafiaHub>>();
                hubContext.Clients
                    .Users(ConnectedPlayerIds.Keys.ToList())
                    .SendAsync("game-state", GameStatePacket());
            }
        }

        public MafiaConnectInfo ConnectInfo()
        {
            return new MafiaConnectInfo
            { 
                Id = Id,
                Name = Name,
                Capacity = $"{MaxPlayers}/{ConnectedPlayerIds.Count}"
            };
        }

        private object GameStatePacket()
        {
            return new
            {
                State,
            };
        }

        private class Player
        {
            public Player(string userId, int gameId, string name, bool connected)
            {
                UserId = userId;
                PlayerId = gameId;
                Name = name;
                Connected = connected;
            }

            public string UserId { get; set; }
            public int PlayerId { get; set; }
            public string Name { get; set; }
            public bool Connected { get; set; }
            public bool IsHost { get; set; }
        }
    }
}
