using Boadr.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boadr.Domain.Models.Mafia
{
    public class MafiaStatePacket
    {
        public GameState State { get; set; }
        public IEnumerable<MafiaPlayer> Players { get; set; }
    }
}
