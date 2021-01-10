using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KatieSoccer.Server.Accessors;
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

        public GameHub(IGameAccessor gameAccessor)
        {
            GameAccessor = gameAccessor;
        }

        private IGameAccessor GameAccessor { get; }

        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.All.SendAsync("JoinedGame", gameId);
        }

        public async Task SetPlayerConnectionId(string gameId)
        {
            await GameAccessor.SetPlayerConnectionId(gameId, Context.ConnectionId);
        }

        public async Task InitializeGame(string gameId)
        {
            var data = await GameAccessor.GetGame(gameId);

            var playerOneLocal = false;
            var playerTwoLocal = false;
            
            if (data.IsOnline && data.PlayerOne.ConnectionId.Equals(Context.ConnectionId))
            {
                playerOneLocal = true;
                playerTwoLocal = false;
            }
            else if (data.IsOnline
                && data.PlayerTwo != null
                && data.PlayerTwo.ConnectionId.Equals(Context.ConnectionId))
            {
                playerOneLocal = false;
                playerTwoLocal = true;
            }
            else if (!data.IsOnline)
            {
                playerOneLocal = true;
                playerTwoLocal = true;
            }

            var dataJson = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, data);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                dataJson = await reader.ReadToEndAsync();
            };

            await Clients.Group(gameId).SendAsync("GameInitialized", dataJson);
            await SetLocalPlayers(playerOneLocal, playerTwoLocal);
        }

        public async Task SetLocalPlayers(bool playerOne, bool playerTwo)
        {
            var data = new { PlayerOneLocal = playerOne, PlayerTwoLocal = playerTwo };
            var dataJson = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, data);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                dataJson = await reader.ReadToEndAsync();
            };

            await Clients.Client(Context.ConnectionId).SendAsync("LocalPlayersSet", dataJson);
        }

        public async Task AddTurn(string dataJson)
        {
            var data = JsonSerializer.Deserialize<TurnData>(dataJson, jsonSerializerOptions);
            await Clients.Group(data.GameId).SendAsync("TurnReceived", dataJson);
        }

        public async Task UpdateScore(string dataJson)
        {
            var data = JsonSerializer.Deserialize<ScoreData>(dataJson, jsonSerializerOptions);
            await Clients.Group(data.GameId).SendAsync("ScoreReceived", data);
        }
    }
}
