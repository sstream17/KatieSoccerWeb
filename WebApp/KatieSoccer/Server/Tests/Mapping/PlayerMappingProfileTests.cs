using System.Text.Json;
using AutoMapper;
using KatieSoccer.Server.Accessors.Mapping;
using Xunit;

namespace KatieSoccer.Server.Accessors.Tests.Mapping
{
    public class PlayerMappingProfileTests
    {
        public PlayerMappingProfileTests()
        {
            var mapperConfiguration = new MapperConfiguration(
                config =>
                {
                    config.AddMaps(typeof(PlayerMappingProfile));
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
        public void PlayerToPlayerModel_ShouldMap()
        {
            var testPlayer = new Shared.Player
            {
                Color = "red",
                IsLocal = true,
                Name = "Player1"
            };

            var expectedPlayerModel = new EntityFramework.Models.Player
            {
                Color = "red",
                IsLocal = 1,
                Name = "Player1"
            };

            var actualPlayerModel = Mapper.Map<EntityFramework.Models.Player>(testPlayer);

            // Serializing as JSON so the objects can be directly compared
            var serializedPlayerModel = JsonSerializer.Serialize(actualPlayerModel);
            var expectedSerializedPlayerModel = JsonSerializer.Serialize(expectedPlayerModel);
            Assert.Equal(expectedSerializedPlayerModel, serializedPlayerModel);
        }

        [Fact]
        public void PlayerModelToPlayer_ShouldMap()
        {
            var testPlayerModel = new EntityFramework.Models.Player
            {
                Color = "red",
                IsLocal = 1,
                Name = "Player1"
            };

            var expectedPlayer = new Shared.Player
            {
                Color = "red",
                IsLocal = true,
                Name = "Player1"
            };

            var actualPlayer = Mapper.Map<Shared.Player>(testPlayerModel);

            // Serializing as JSON so the objects can be directly compared
            var serializedPlayer = JsonSerializer.Serialize(actualPlayer);
            var expectedSerializedPlayer = JsonSerializer.Serialize(expectedPlayer);
            Assert.Equal(expectedSerializedPlayer, serializedPlayer);
        }
    }
}
