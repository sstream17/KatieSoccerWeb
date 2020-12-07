using System.Text.Json;
using System.Threading.Tasks;
using KatieSoccer.Shared;
using Microsoft.AspNetCore.SignalR;

namespace KatieSoccer.Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.All.SendAsync("JoinedGame", gameId);
        }

        public async Task AddTurn(string dataJson)
        {
            var data = JsonSerializer.Deserialize<TurnData>(dataJson);
            await Clients.Group(data.GameId).SendAsync("TurnReceived", dataJson);
        }
    }
}
