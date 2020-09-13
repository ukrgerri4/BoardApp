using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardApp.Hubs
{
    [Authorize]
    public class ResistanceHub: Hub
    {
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
            await base.OnConnectedAsync();
        }
    }
}
