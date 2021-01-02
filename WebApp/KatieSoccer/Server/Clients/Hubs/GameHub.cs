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

        public async Task InitializeGame(string gameId)
        {
            var data = new GameData
            {
                GameId = gameId,
                PlayerOne = new Player
                {
                    IsLocal = true,
                    Name = "Team One",
                    Color = "#a72b2a"
                },
                PlayerTwo = new Player
                {
                    IsLocal = false,
                    Name = "Team Two",
                    Color = "#debc97"
                }
            };

            var gameCreated = await GameAccessor.EnsureGameCreated(data);

            data.PlayerOne.IsLocal = gameCreated;
            data.PlayerTwo.IsLocal = !gameCreated;

            var dataJson = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, data);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                dataJson = await reader.ReadToEndAsync();
            };

            await Clients.All.SendAsync("GameInitialized", dataJson);
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
