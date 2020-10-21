using Board.Application.Enumerations;
using Board.Application.Interfaces.Services;
using Board.Common.Models;
using Board.Infrastructure.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Board.Infrastructure.Hubs
{
    [Authorize]
    public class MafiaHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MafiaHub(IServiceScopeFactory serviceScopeFactory, IGameService gameService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _gameService = gameService;
        }

        public HubResult Games()
        {
            var avalibleGames = _gameService.LiveGames
                .Where(g => g.Value.State == GameState.Created)
                .Select(g => new 
                {
                    g.Value.Id,
                    g.Value.Name,
                    g.Value.State
                })
                .ToList();
            return HubResult.Ok(new { avalible = avalibleGames });
        }

        public HubResult CreateGame()
        {
            if (_gameService.UsersInGame.ContainsKey(Context.UserIdentifier))
            {
                return HubResult.Fail("User already in game.");
            }
            var game = new MafiaGame(_serviceScopeFactory);
            
            _gameService.LiveGames.TryAdd(game.Id, game);
            _gameService.UsersInGame.TryAdd(Context.UserIdentifier, game.Id);
            
            game.AddPlayer(Context.UserIdentifier);
            game.Update();

            return HubResult.Ok(game.Id);
        }

        //public HubResult JoinGame()
        //{

        //}


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "connected");

            _gameService.RemovePlayer(Context.UserIdentifier);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
