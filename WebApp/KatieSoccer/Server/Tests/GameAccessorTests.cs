using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using KatieSoccer.Shared;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace KatieSoccer.Server.Accessors.Tests
{
    public class GameAccessorTests
    {
        private List<Game> EmptyGames => new List<Game>();

        private List<Game> Games => new List<Game>
        {
            new Game
            {
                GameId = "testgameId",
                IsOnline = false,
                PlayerOne = new EntityFramework.Models.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "player1"
                },
                PlayerTwo = new EntityFramework.Models.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "player2"
                }
            },
            new Game
            {
                GameId = "testgameId2",
                IsOnline = true,
                PlayerOne = new EntityFramework.Models.Player
                {
                    Color = "red",
                    IsLocal = false,
                    Name = "player3"
                },
                PlayerTwo = new EntityFramework.Models.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "player4"
                }
            }
        };

        private Game TestGame => new Game
        {
            GameId = "testgameId",
            PlayerOne = new EntityFramework.Models.Player
            {
                Color = "red",
                IsLocal = true,
                Name = "player1"
            },
            PlayerTwo = new EntityFramework.Models.Player
            {
                Color = "blue",
                IsLocal = false,
                Name = "player2"
            }
        };

        private List<GameData> TestGameDatas => new List<GameData>
        {
            new GameData
            {
                GameId = "testgameId",
                IsOnline = false,
                PlayerOne = new Shared.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "player1"
                },
                PlayerTwo = new Shared.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "player2"
                }
            },
            new GameData
            {
                GameId = "testgameId2",
                IsOnline = true,
                PlayerOne = new Shared.Player
                {
                    Color = "red",
                    IsLocal = false,
                    Name = "player3"
                },
                PlayerTwo = new Shared.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "player4"
                }
            }
        };

        [Fact]
        public async Task AddGame_ShouldSucceed()
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(m => m.Map<Game>(It.IsAny<GameData>()))
                .Returns(TestGame);

            var mockDbSet = EmptyGames.AsQueryable().BuildMockDbSet();
            var mockDbContext = new Mock<IKatieSoccerDbContext>();
            mockDbContext
                .Setup(db => db.Games)
                .Returns(mockDbSet.Object);

            var gameAccessor = new GameAccessor(mockMapper.Object, mockDbContext.Object);

            var gameToAdd = new GameData
            {
                GameId = "testgameId",
                PlayerOne = new Shared.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "player1"
                },
                PlayerTwo = new Shared.Player
                {
                    Color = "blue",
                    IsLocal = false,
                    Name = "player2"
                }
            };

            await gameAccessor.AddGame(gameToAdd);
        }

        [Fact]
        public async Task GetGames_ReturnsGameDatas()
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(m => m.Map<List<GameData>>(It.IsAny<List<Game>>()))
                .Returns(TestGameDatas);

            var mockDbSet = Games.AsQueryable().BuildMockDbSet();
            var mockDbContext = new Mock<IKatieSoccerDbContext>();
            mockDbContext
                .Setup(db => db.Games)
                .Returns(mockDbSet.Object);

            var gameAccessor = new GameAccessor(mockMapper.Object, mockDbContext.Object);

            var actualGameDatas = await gameAccessor.GetGames();

            // Serializing as JSON so the objects can be directly compared
            var serializedGameDatas = JsonSerializer.Serialize(actualGameDatas);
            var expectedSerializedGameDatas = JsonSerializer.Serialize(TestGameDatas);
            Assert.Equal(expectedSerializedGameDatas, serializedGameDatas);
        }
    }
}
