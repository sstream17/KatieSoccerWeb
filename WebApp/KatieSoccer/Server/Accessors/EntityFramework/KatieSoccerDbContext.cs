﻿using KatieSoccer.Server.Accessors.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors.EntityFramework
{
    public class KatieSoccerDbContext : DbContext, IKatieSoccerDbContext
    {
        public KatieSoccerDbContext(DbContextOptions<KatieSoccerDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Player> Players { get; set; }
    }
}
