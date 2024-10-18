﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Models;

namespace SeminarHub.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
        }

		public virtual DbSet<Category> Categories { get; set; } = null!;
		public virtual DbSet<Seminar> Seminars { get; set; } = null!;
		public virtual DbSet<SeminarParticipant> SeminarsParticipants { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder builder)
        {

			//PK
			//builder.Entity<SeminarParticipant>()
			//	.HasKey(pk => new { pk.ParticipantId, pk.SeminarId });

			//Seed
			builder
			   .Entity<Category>()
			   .HasData(new Category()
			   {
				   Id = 1,
				   Name = "Technology & Innovation"
			   },
			   new Category()
			   {
				   Id = 2,
				   Name = "Business & Entrepreneurship"
			   },
			   new Category()
			   {
				   Id = 3,
				   Name = "Science & Research"
			   },
			   new Category()
			   {
				   Id = 4,
				   Name = "Arts & Culture"
			   });

			base.OnModelCreating(builder);
        }
    }
}