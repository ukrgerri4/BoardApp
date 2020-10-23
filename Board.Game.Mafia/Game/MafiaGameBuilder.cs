using Board.Game.Mafia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Board.Game.Mafia.Game
{
    public interface IMafiaGameBuilder
    {
        MafiaGame Build();
        MafiaGameBuilder ContainsPlayer(IdentityUser user);
        MafiaGameBuilder CreateGame();
        MafiaGameBuilder HasMaxPlayers(int maxPlayers);
        MafiaGameBuilder WithName(string name);
        MafiaGameBuilder WithState(MafiaGameState state);
    }

    public class MafiaGameBuilder : IMafiaGameBuilder
    {
        private MafiaGame _game;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MafiaGameBuilder(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public MafiaGameBuilder CreateGame()
        {
            _game = null;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _game = scope.ServiceProvider.GetRequiredService<MafiaGame>();
                _game.Id = Guid.NewGuid().ToString();
                _game.Name = $"Huba-buba";
                _game.State = MafiaGameState.Created;
            }
            return this;
        }

        public MafiaGameBuilder WithName(string name)
        {
            _game.Name = name;
            return this;
        }

        public MafiaGameBuilder HasMaxPlayers(int maxPlayers)
        {
            _game.MaxPlayers = maxPlayers;
            return this;
        }

        public MafiaGameBuilder WithState(MafiaGameState state)
        {
            _game.State = state;
            return this;
        }

        public MafiaGameBuilder ContainsPlayer(IdentityUser user)
        {
            _game.AddPlayer(user);
            return this;
        }

        public MafiaGame Build() => _game;
    }
}
