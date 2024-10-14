using CinemaApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaApp.Data
{
	public class CinemaDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
	{
		public CinemaDbContext()
		{

		}

		public CinemaDbContext(DbContextOptions options)
		: base(options)
		{
		}

		public virtual DbSet<Movie> Movies { get; set; } = null!;
		public virtual DbSet<Cinema> Cinemas { get; set; } = null!;
		public virtual DbSet<CinemaMovie> CinemasMovies { get; set; } = null!;
		public virtual DbSet<ApplicationUserMovie> UserMovies { get; set; } = null!;



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{	
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		
	}
}
