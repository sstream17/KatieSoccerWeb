using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors.EntityFramework
{
    public interface IKatieSoccerDbContext
    {
        DbSet<Game> Games { get; set; }

        int SaveChanges();
    }
}
