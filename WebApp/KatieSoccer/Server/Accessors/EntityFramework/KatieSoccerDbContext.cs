using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors.EntityFramework
{
    public class KatieSoccerDbContext : DbContext, IKatieSoccerDbContext
    {
        public KatieSoccerDbContext(DbContextOptions<KatieSoccerDbContext> options) : base(options)
        {
            Database.EnsureCreatedAsync();
        }

        public virtual DbSet<Game> Games { get; set; }
    }
}
