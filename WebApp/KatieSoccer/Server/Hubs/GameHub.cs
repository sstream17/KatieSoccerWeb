﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace KatieSoccer.Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task AddForce()
        {
        }
    }
}
