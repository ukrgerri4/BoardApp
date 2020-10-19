using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Board.Infrastructure.Hubs
{
    [Authorize]
    public class ResistanceHub : Hub
    {
        private ResistanceState _state = new ResistanceState
        {
            Phase = 1,
            Round = 2,
            Players = new ResistancePlayer[]
            {
                new ResistancePlayer { Id = "1", Name = "Igor", IsHiking = true },
                new ResistancePlayer { Id = "2", Name = "Zepsen", IsHiking = false },
                new ResistancePlayer { Id = "3", Name = "Marina", IsHiking = false },
                new ResistancePlayer { Id = "4", Name = "Yulia", IsHiking = false },
                new ResistancePlayer { Id = "5", Name = "Stas", IsBoss = true }
            }
        };

        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }

        public async Task<string> CreateGame()
        {
            return Guid.NewGuid().ToString();
        }


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "all_users");
            await Clients.Group("all_users").SendAsync("game-state", _state);
            await base.OnConnectedAsync();
        }
    }

    public class ResistanceState
    {
        [JsonPropertyName("ph")]
        public byte Phase { get; set; }

        [JsonPropertyName("rd")]
        public byte Round { get; set; }

        [JsonPropertyName("pr")]
        public ResistancePlayer[] Players { get; set; }
    }

    public class ResistancePlayer
    {
        [JsonPropertyName("i")]
        public string Id { get; set; }

        [JsonPropertyName("n")]
        public string Name { get; set; }

        [JsonPropertyName("b")]
        public bool? IsBoss { get; set; }

        [JsonPropertyName("h")]
        public bool? IsHiking { get; set; }

        [JsonPropertyName("v")]
        public byte Vote { get; set; }
    }
}
