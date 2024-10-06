using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaApp.Data.Configuration
{
	class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
	{
		public void Configure(EntityTypeBuilder<CinemaMovie> builder)
		{	
			//Mapping table PK!
			builder.HasKey(pk => new { pk.MovieId,pk.CinemaId});

			builder.Property(cm => cm.IsDeleted)
				   .HasDefaultValue(false);

			builder.HasOne(cm => cm.Movie)
				   .WithMany(m => m.MoviesCinema)
				   .HasForeignKey(cm => cm.MovieId)
			       .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(cm => cm.Cinema)
				   .WithMany(c => c.CinemaMovies)
				   .HasForeignKey(cm => cm.CinemaId)
				   .OnDelete(DeleteBehavior.Restrict); ;

		}

	}
}
