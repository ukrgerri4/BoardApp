namespace Boadr.Domain.Models.Mafia
{
    public class MafiaPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public bool IsLive { get; set; }
    }
}
