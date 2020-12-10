using System.Collections.Generic;
using System.Threading.Tasks;
using KatieSoccer.Server.Accessors.EntityFramework.Models;

namespace KatieSoccer.Server.Accessors
{
    public interface IGameAccessor
    {
        Task AddGame(Game game);

        Task<List<Game>> GetGames();
    }
}
