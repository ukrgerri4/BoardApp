using Board.Application.Interfaces;
using Board.Application.Interfaces.Services;
using Board.Game.Mafia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Board.Game.Mafia
{
    public class MafiaGame : IGame
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IActiveUserService _activeUserService;
        public string Id { get; set; }
        public string Name { get; set; }
        public MafiaGameState State { get; set; }


        public int MaxPlayers { get; set; }
        private Dictionary<string, Player> ConnectedPlayers { get; set; }
        
        private Subject<Unit> _notifyer = new Subject<Unit>();
        private Subject<Unit> _destroy = new Subject<Unit>();

        public MafiaGame(IServiceScopeFactory serviceScopeFactory, IActiveUserService activeUserService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _activeUserService = activeUserService;

            ConnectedPlayers = new Dictionary<string, Player>();

            _activeUserService.OnChange()
                .TakeUntil(_destroy)
                .Where(userInfo => ConnectedPlayers.ContainsKey(userInfo.UserId))
                .Do(_ => Console.WriteLine($"Game - {Id}. Find user - {_.UserId} - {_.Connected}"))
                .Subscribe(userInfo =>
                {
                    ConnectedPlayers[userInfo.UserId].Connected = userInfo.Connected;
                    Update();
                });

            _notifyer.AsObservable()
                .TakeUntil(_destroy)
                .Sample(TimeSpan.FromMilliseconds(500))
                .Where(_ => State != MafiaGameState.Ended)
                .Do(_ => Console.WriteLine($"Game - {Id}"))
                .Subscribe(_ => Notify());
        }

        public void Start()
        {
            State = MafiaGameState.Started;
        }

        public void Pause()
        {
            State = MafiaGameState.Paused;
        }

        public void End()
        {
            State = MafiaGameState.Ended;
            _destroy.OnNext(Unit.Default);
            _destroy.OnCompleted();
            _notifyer.OnCompleted();
        }

        public void Update()
        {
            _notifyer.OnNext(Unit.Default);
        }

        public void AddPlayer(IdentityUser user)
        {
             ConnectedPlayers.TryAdd(user.Id, new Player(user));
            _activeUserService.AddGameConnection(user.Id, Id);
            Update();
        }

        public void RemovePlayer(string playerId)
        {
            ConnectedPlayers.Remove(playerId);
            Update();
        }

        public bool ContainsPlayer(string userId)
        {
            return ConnectedPlayers.ContainsKey(userId);
        }

        private void Notify()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MafiaHub>>();
                hubContext.Clients
                    .Users(ConnectedPlayers.Keys.ToList())
                    .SendAsync("upd-state", GameStatePacket());
            }
        }

        public MafiaConnectInfo ConnectInfo()
        {
            return new MafiaConnectInfo
            { 
                Id = Id,
                Name = Name,
                Capacity = $"{MaxPlayers}/{ConnectedPlayers.Count}"
            };
        }

        public object GameStatePacket()
        {
            return new MafiaStatePacket
            {
                State = State,
                Players = ConnectedPlayers.Values
                    .Where(p => p.Connected)
                    .Select(p => new MafiaPlayer
                    {
                        Id = p.UserId,
                        Name = p.Name,
                        IsLive = p.IsLive,
                        Role = (int)p.Role,
                        Connected = p.Connected
                    })
            };
        }

        private class Player
        {
            public Player(IdentityUser user)
            {
                UserId = user.Id;
                Name = user.UserName;
                Connected = true;
                IsLive = true;
                IsHost = false;
                Role = MafiaRole.Undefined;
            }

            public string UserId { get; set; }
            public string Name { get; set; }
            public MafiaRole Role { get; set; }
            public bool Connected { get; set; }
            public bool IsLive { get; set; }
            public bool IsHost { get; set; }
        }

        private enum MafiaRole
        {
            Undefined,
            Сivilian,
            Chief,
            Doctor,
            Mafia,
            DonMafia,
            Maniac,
            Whore
        }
    }
}
