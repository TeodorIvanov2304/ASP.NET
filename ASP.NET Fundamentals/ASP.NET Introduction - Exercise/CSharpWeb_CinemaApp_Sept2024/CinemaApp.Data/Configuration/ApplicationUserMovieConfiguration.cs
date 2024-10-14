using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
	public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserMovie> builder)
		{
			builder.HasKey(um => new { um.ApplicationUserId, um.MovieId });

			builder.HasOne(um => um.Movie)
					.WithMany(m => m.MovieApplicationUsers)
					.HasForeignKey(m => m.MovieId);

			builder.HasOne(um => um.ApplicationUser)
				   .WithMany(m => m.ApplicationUserMovies)
				   .HasForeignKey(um => um.ApplicationUserId);
		}
	}
}
