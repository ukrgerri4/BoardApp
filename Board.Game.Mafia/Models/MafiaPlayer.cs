namespace Board.Game.Mafia.Models
{
    public class MafiaPlayer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public bool IsLive { get; set; }
        public bool Connected { get; set; }
    }
}
