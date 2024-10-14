using CinemaApp.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CinemaApp.Common.EntityValidationConstants.Movie;
using static CinemaApp.Common.ApplicationConstants;

namespace CinemaApp.Data.Configuration
{
	public class MovieConfiguration : IEntityTypeConfiguration<Movie>
	{
		public void Configure(EntityTypeBuilder<Movie> builder)
		{
			//Fluent API
			builder.HasKey(m=>m.Id);
			builder.Property(m => m.Title)
				.IsRequired()
				.HasMaxLength(TitleMaxLength);

			builder.Property(m => m.Genre)
				.IsRequired()
				.HasMaxLength(GenreMaxLength);

			builder.Property(m => m.Director)
				.IsRequired()
				.HasMaxLength(DirectorNameMaxLength);

			builder.Property(m => m.Description)
				.IsRequired()
				.HasMaxLength(DescriptionNameMaxLength);

			builder.Property(m => m.ImageUrl)
				   .IsRequired(false)
				   .HasMaxLength(ImageUrlMaxLength)
				   .HasDefaultValue(NoImageUrl);

			builder.HasData(this.SeedMovies());
		}

		private IEnumerable<Movie> SeedMovies()
		{
			IEnumerable<Movie> movies = new List<Movie>()
			{
				new Movie()
				{
					Title = "Harry Potter and the Goblet of fire",
					Genre = "Fantasy",
					ReleaseDate = new DateTime(2005,11,01),
					Director = "Mike Newel",
					Duration = 157,
					Description = "In his fourth year at Hogwarts, Harry must reluctantly compete in an ancient wizard tournament after someone mysteriously selects his name, while the Dark Lord secretly conspires something sinister."
				},
				new Movie()
				{
					Title = "Lord of the Rings",
					Genre = "Fantasy",
					ReleaseDate = new DateTime(2001,05,01),
					Director = "Peter Jackson",
					Duration = 178,
					Description = "Among motion pictures of Middle-earth in various formats, The Lord of the Rings is a trilogy of epic fantasy adventure films directed by Peter Jackson, based on the novel The Lord of the Rings by British author J. R. R. Tolkien."
				}
			};

			return movies;
		}
	}
}
