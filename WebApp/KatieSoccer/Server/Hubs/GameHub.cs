using System.Text.Json;
using System.Threading.Tasks;
using KatieSoccer.Shared;
using Microsoft.AspNetCore.SignalR;

namespace KatieSoccer.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.All.SendAsync("JoinedGame", gameId);
        }

        public async Task AddTurn(string dataJson)
        {
            var data = JsonSerializer.Deserialize<TurnData>(dataJson, jsonSerializerOptions);
            await Clients.Group(data.GameId).SendAsync("TurnReceived", dataJson);
        }

        public async Task UpdateScore(string scoreJson)
        {
            var data = JsonSerializer.Deserialize<ScoreData>(scoreJson, jsonSerializerOptions);
            await Clients.Group(data.GameId).SendAsync("ScoreReceived", data);
        }
    }
}
