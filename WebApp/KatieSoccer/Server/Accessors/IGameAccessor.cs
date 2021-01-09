using System.Collections.Generic;
using System.Threading.Tasks;
using KatieSoccer.Shared;

namespace KatieSoccer.Server.Accessors
{
    public interface IGameAccessor
    {
        Task<bool> EnsureGameCreated(GameData gameData);

        Task AddGame(GameData game);

        Task<bool> AddPlayer(string gameId, Player playerToAdd);

        Task<GameData> GetGame(string gameId);

        Task<List<GameData>> GetGames();
    }
}
