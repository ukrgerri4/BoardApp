using System.Collections.Generic;

namespace Board.Game.Mafia.Models
{
    public class MafiaStatePacket
    {
        public MafiaGameState State { get; set; }
        public IEnumerable<MafiaPlayer> Players { get; set; }
    }
}
