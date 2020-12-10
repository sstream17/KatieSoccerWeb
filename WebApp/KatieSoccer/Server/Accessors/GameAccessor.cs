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

        public async Task AddGame(Game game)
        {
            try
            {
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
