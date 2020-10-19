using Board.Domain.Definitions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Board.Infrastructure.Services.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        string IUserIdProvider.GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Claims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Id)?.Value;
        }
    }
}
