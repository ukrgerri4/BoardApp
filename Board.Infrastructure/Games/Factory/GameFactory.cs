using Board.Application.Interfaces.Games.Factory;
using Board.Application.Interfaces.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Board.Infrastructure.Games.Factory
{
    public class GameFactory: IGameFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public GameFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public T Create<T>() where T: IGame
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }
    }
}
