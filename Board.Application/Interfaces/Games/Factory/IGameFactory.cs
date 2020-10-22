using Board.Application.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Board.Application.Interfaces.Games.Factory
{
    public interface IGameFactory
    {
        T Create<T>() where T : IGame;
    }
}
