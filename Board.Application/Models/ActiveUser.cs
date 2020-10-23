using System;
using System.Collections.Generic;
using System.Linq;

namespace Board.Application.Models
{
    public class ActiveUser
    {
        public ActiveUser(string userId, string name, IEnumerable<string> connections = null)
        {
            UserId = userId;
            Name = name;
            Connections = connections != null ? new HashSet<string>(connections) : new HashSet<string>();
            Games = new HashSet<string>();
            StartOffline = null;
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public HashSet<string> Connections { get; set; }
        public HashSet<string> Games { get; set; }

        public DateTime? StartOffline { get; set; }
        
        public bool Connected => Connections.Any();

        public void AddConnection(string connectionId)
        {
            Connections.Add(connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            Connections.Remove(connectionId);
        }

        public void AddGame(string connectionId)
        {
            Games.Add(connectionId);
        }

        public void RemoveGame(string connectionId)
        {
            Games.Remove(connectionId);
        }
    }
}
