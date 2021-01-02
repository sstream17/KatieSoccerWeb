using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.Azure.Cosmos;
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

        public async Task<bool> EnsureGameCreated(Shared.GameData gameData)
        {
            Game game = null;
            try
            {
                game = await KatieSoccerDbContext
                    .Games
                    .FindAsync(gameData.GameId)
                    .ConfigureAwait(false);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
            }

            var gameCreated = false;

            if (game == null)
            {
                await AddGame(gameData);
                gameCreated = true;
            }

            return gameCreated;
        }

        public async Task AddGame(Shared.GameData gameData)
        {
            try
            {
                var game = Mapper.Map<Game>(gameData);

                await KatieSoccerDbContext
                    .Games
                    .AddAsync(game)
                    .ConfigureAwait(false);

                await KatieSoccerDbContext.SaveChangesAsync();
            }
            catch (Exception e)
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
