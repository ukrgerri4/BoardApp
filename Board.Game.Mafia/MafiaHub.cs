using Board.Application.Interfaces;
using Board.Application.Interfaces.Services;
using Board.Common.Models;
using Board.Game.Mafia.Game;
using Board.Game.Mafia.Models;
using Board.Game.Mafia.Models.Hub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Board.Game.Mafia
{
    [Authorize]
    public class MafiaHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly IActiveUserService _activeUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMafiaGameBuilder _mafiaGameBuilder;

        public MafiaHub(
            IActiveUserService activeUserService,
            IGameService gameService,
            UserManager<IdentityUser> userManager,
            IMafiaGameBuilder mafiaGameBuilder)
        {
            _gameService = gameService;
            _activeUserService = activeUserService;
            _userManager = userManager;
            _mafiaGameBuilder = mafiaGameBuilder;
        }

        [HubMethodName("host-games")]
        public HubResult GetGames()
        {
            var activeGames = _gameService.Games
                .Select(d => d.Value)
                .Where(g => 
                    g is MafiaGame game
                    && game.State == MafiaGameState.Created 
                    && game.ContainsPlayer(Context.UserIdentifier)
                )
                .Select(g => (g as MafiaGame).ConnectInfo())
                .ToList();

            var avalibleGames = _gameService.Games
                .Select(d => d.Value)
                .Where(g => g is MafiaGame game 
                    && game.State == MafiaGameState.Created
                    && !game.ContainsPlayer(Context.UserIdentifier))
                .Select(g => (g as MafiaGame).ConnectInfo())
                .ToList();
            return HubResult.Ok(new { active = activeGames, avalible = avalibleGames });
        }

        [HubMethodName("create")]
        public async Task<HubResult> CreateGame(MafiaCreateGameOptions options)
        {
            if (_activeUserService.HasGameConnections(Context.UserIdentifier))
            {
                return HubResult.Fail("User already in game.");
            }

            var user = await _userManager.FindByIdAsync(Context.UserIdentifier);

            var game = _mafiaGameBuilder
                .CreateGame()
                .WithName(options.Name)
                .HasMaxPlayers(options.MaxPlayers)
                .ContainsPlayer(user)
                .Build();
            
            _gameService.AddGame(game);

            return HubResult.Ok(game.Id);
        }

        [HubMethodName("join")]
        public async Task<HubResult> JoinGame(string gameId)
        {
            if (_activeUserService.HasGameConnections(Context.UserIdentifier))
            {
                return HubResult.Fail("User already in game.");
            }

            _gameService.Games.TryGetValue(gameId, out IGame game);
            var user = await _userManager.FindByIdAsync(Context.UserIdentifier);

            if (game == null || user == null)
            {
                return HubResult.Fail("Game not found.");
            }

            game.AddPlayer(user);
            return HubResult.Ok(game.Id);
        }

        [HubMethodName("state")]
        public HubResult GetState(string gameId)
        {
            _gameService.Games.TryGetValue(gameId, out IGame game);
            if (game == null) return HubResult.Fail("Game not found.");

            Clients.User(Context.UserIdentifier).SendAsync("upd-state", game.GameStatePacket());

            return HubResult.Ok();
        }

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
