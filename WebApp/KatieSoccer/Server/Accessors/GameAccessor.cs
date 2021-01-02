using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors
{
    public class GameAccessor : IGameAccessor
    {
        public GameAccessor(IMapper mapper, IKatieSoccerDbContext katieSoccerDbContext)
        {
            Mapper = mapper;
            KatieSoccerDbContext = katieSoccerDbContext;
        }

        private IMapper Mapper { get; }

        private IKatieSoccerDbContext KatieSoccerDbContext { get; }

        public async Task AddGame(Shared.GameData gameData)
        {
            try
            {
                var game = Mapper.Map<Game>(gameData);

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

        public async Task<List<Shared.GameData>> GetGames()
        {
            try
            {
                var games = await KatieSoccerDbContext
                    .Games
                    .ToListAsync()
                    .ConfigureAwait(false);

                return Mapper.Map<List<Shared.GameData>>(games);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
