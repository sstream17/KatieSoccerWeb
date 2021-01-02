using System.Text.Json;
using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using KatieSoccer.Server.Accessors.Mapping;
using KatieSoccer.Shared;
using Xunit;

namespace KatieSoccer.Server.Accessors.Tests.Mapping
{
    public class GameMappingProfileTests
    {
        public GameMappingProfileTests()
        {
            var mapperConfiguration = new MapperConfiguration(
                config =>
                {
                    config.AddMaps(typeof(GameMappingProfile));
                });

            Mapper = mapperConfiguration.CreateMapper();
        }

        private IMapper Mapper { get; }

        [Fact]
        public void PlayerMappingProfile_ConfigurationIsValid()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void GameDataToGame_ShouldMap()
        {
            var testGameData = new GameData
            {
                GameId = "testGameId",
                IsOnline = true,
                PlayerOne = new Shared.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "Player1"
                },
                PlayerTwo = new Shared.Player
                {
                    Color = "blue",
                    IsLocal = false,
                    Name = "Player2"
                }
            };

            var expectedGame = new Game
            {
                GameId = "testGameId",
                IsOnline = true,
                PlayerOne = new EntityFramework.Models.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "Player1"
                },
                PlayerTwo = new EntityFramework.Models.Player
                {
                    Color = "blue",
                    IsLocal = false,
                    Name = "Player2"
                }
            };

            var actualGame = Mapper.Map<Game>(testGameData);

            // Serializing as JSON so the objects can be directly compared
            var serializedGame = JsonSerializer.Serialize(actualGame);
            var expectedSerializedGame = JsonSerializer.Serialize(expectedGame);
            Assert.Equal(expectedSerializedGame, serializedGame);
        }

        [Fact]
        public void GameToGameData_ShouldMap()
        {
            var testGameData = new Game
            {
                GameId = "testGameId",
                IsOnline = false,
                PlayerOne = new EntityFramework.Models.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "Player1"
                },
                PlayerTwo = new EntityFramework.Models.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "Player2"
                }
            };

            var expectedGameData = new GameData
            {
                GameId = "testGameId",
                IsOnline = false,
                PlayerOne = new Shared.Player
                {
                    Color = "red",
                    IsLocal = true,
                    Name = "Player1"
                },
                PlayerTwo = new Shared.Player
                {
                    Color = "blue",
                    IsLocal = true,
                    Name = "Player2"
                }
            };

            var actualGameData = Mapper.Map<Game>(testGameData);

            // Serializing as JSON so the objects can be directly compared
            var serializedGameData = JsonSerializer.Serialize(actualGameData);
            var expectedSerializedGameData = JsonSerializer.Serialize(expectedGameData);
            Assert.Equal(expectedSerializedGameData, serializedGameData);
        }
    }
}
