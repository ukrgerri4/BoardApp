﻿using Board.Application.Enumerations;
using Board.Application.Interfaces.Models;
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
        public string Id { get; set; }
        public string Name { get; set; }
        public GameState State { get; set; }


        private List<string> ConnectedPlayerIds { get; set; }
        private List<string> StartPlayerIds { get; set; }

        private Subject<Unit> _notifyer = new Subject<Unit>();
        private Subject<Unit> _destroy = new Subject<Unit>();


        public MafiaGame(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            Id = Guid.NewGuid().ToString();
            Name = $"Game-{Id}";
            State = GameState.Created;
            ConnectedPlayerIds = new List<string>();
            StartPlayerIds = new List<string>();

            //Observable.Interval(TimeSpan.FromMilliseconds(1000))
            //    .TakeUntil(_destroy)
            //    .Finally(() => Console.WriteLine($"Stop broadcst game ID - [{Id}]"))
            //    .Subscribe(_ => _notifyer.OnNext(Unit.Default));

            _notifyer.AsObservable()
                .TakeUntil(_destroy)
                .Sample(TimeSpan.FromMilliseconds(1000))
                .Where(_ => State != GameState.Ended)
                .Do(_ => Console.WriteLine($"Send - {Id}"))
                .Subscribe(_ => Notify());
        }

        public void Start()
        {
            State = GameState.Started;
            StartPlayerIds = ConnectedPlayerIds.ToList();
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

        public void AddPlayer(string playerId)
        {
            ConnectedPlayerIds.Add(playerId);
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
                hubContext.Clients.Users(ConnectedPlayerIds.ToList()).SendAsync("game-state", "test");
            }
        }
    }
}
