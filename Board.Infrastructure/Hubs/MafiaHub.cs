using Boadr.Domain.Enumerations;
using Board.Application.Interfaces.Games.Factory;
using Board.Application.Interfaces.Services;
using Board.Common.Models;
using Board.Infrastructure.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Board.Infrastructure.Hubs
{
    [Authorize]
    public class MafiaHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly IActiveUserService _activeUserService;
        private readonly IGameFactory _gameFactory;

        public MafiaHub(
            IActiveUserService activeUserService,
            IGameService gameService,
            IGameFactory gameFactory)
        {
            _gameService = gameService;
            _activeUserService = activeUserService;
            _gameFactory = gameFactory;
        }

        public HubResult Games()
        {
            var avalibleGames = _gameService.Games
                .Where(g => g is MafiaGame)
                .Where(g => g.State == GameState.Created)
                .Select(g => (g as MafiaGame).ConnectInfo())
                .ToList();
            return HubResult.Ok(new { avalible = avalibleGames });
        }

        public HubResult CreateGame(string options)
        {
            if (_activeUserService.HasGameConnections(Context.UserIdentifier))
            {
                return HubResult.Fail("User already in game.");
            }

            _gameService.AddGame(game);

            var game = _gameFactory.Create<MafiaGame>();
            game.AddPlayer(Context.UserIdentifier);
            
            _activeUserService.AddGameConnection(Context.UserIdentifier, game.Id);
            

            return HubResult.Ok(game.Id);
        }

        //public HubResult JoinGame()
        //{

        //}


        public override async Task OnConnectedAsync()
        {
            await _activeUserService.Connected(Context.UserIdentifier, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _activeUserService.Disconnected(Context.UserIdentifier, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
