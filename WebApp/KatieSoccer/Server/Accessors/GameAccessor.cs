using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KatieSoccer.Server.Accessors.EntityFramework;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors
{
    public class GameAccessor : IGameAccessor
    {
        public GameAccessor(IKatieSoccerDbContext katieSoccerDbContext)
        {
            KatieSoccerDbContext = katieSoccerDbContext;
        }

        private IKatieSoccerDbContext KatieSoccerDbContext { get; }

        public async Task AddGame(Shared.GameData gameData)
        {
            try
            {
                var playerOne = gameData.PlayerOne;
                var playerTwo = gameData.PlayerTwo;
                var game = new Game
                {
                    GameId = "54321",  //TODO: Replace with GameId
                    PlayerOne = new Player
                    {
                        IsLocal = Convert.ToInt32(playerOne.IsLocal),
                        Name = playerOne.Name,
                        Color = playerOne.Color
                    },
                    PlayerTwo = new Player
                    {
                        IsLocal = Convert.ToInt32(playerTwo.IsLocal),
                        Name = playerTwo.Name,
                        Color = playerTwo.Color
                    }
                };

                await KatieSoccerDbContext
                    .Games
                    .AddAsync(game)
                    .ConfigureAwait(false);
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<Game>> GetGames()
        {
            try
            {
                var games = await KatieSoccerDbContext
                    .Games
                    .ToListAsync()
                    .ConfigureAwait(false);

                return games;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
