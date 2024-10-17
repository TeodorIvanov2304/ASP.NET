﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Data
{
    public class GameZoneDbContext : IdentityDbContext
    {
        public GameZoneDbContext(DbContextOptions<GameZoneDbContext> options)
            : base(options)
        {
        }
		public virtual DbSet<Game> Games { get; set; } = null!;
		public virtual DbSet<Genre> Genres { get; set; } = null!;
		public virtual DbSet<GamerGame> GamersGames { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

			//builder.Entity<GamerGame>()
			//	   .HasKey(pk => new { pk.GamerId, pk.GameId });


			builder
				.Entity<Genre>()
				.HasData(
				new Genre { Id = 1, Name = "Action" },
				new Genre { Id = 2, Name = "Adventure" },
				new Genre { Id = 3, Name = "Fighting" },
				new Genre { Id = 4, Name = "Sports" },
				new Genre { Id = 5, Name = "Racing" },
				new Genre { Id = 6, Name = "Strategy" });
		}
	}
}
