using System.Collections.Generic;
using System.Linq;
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

        private Game TestGame => new Game
        {
            GameId = "testgameId",
            PlayerOne = new EntityFramework.Models.Player
            {
                Color = "red",
                IsLocal = 1,
                Name = "player1"
            },
            PlayerTwo = new EntityFramework.Models.Player
            {
                Color = "blue",
                IsLocal = 0,
                Name = "player2"
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
    }
}
