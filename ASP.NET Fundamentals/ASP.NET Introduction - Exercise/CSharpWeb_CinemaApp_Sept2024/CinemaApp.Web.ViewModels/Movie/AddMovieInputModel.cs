using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationConstants.Movie;
using static CinemaApp.Common.EntityValidationMessages.Movie;


namespace CinemaApp.Web.ViewModels.Movie
{
	public class AddMovieInputModel
	{
        public AddMovieInputModel()
        {
			this.ReleaseDate = DateTime.UtcNow.ToString(ReleaseDateFormat);
        }

        [Required(ErrorMessage = TitleRequiredMessage)]
		[MinLength(TitleMinLength)]
		[MaxLength(TitleMaxLength)]
		public string Title { get; set; } = null!;

		[Required(ErrorMessage = GenreRequiredMessage)]
		[MinLength(GenreMinLength)]
		[MaxLength(GenreMaxLength)]
		public string Genre { get; set; } = null!;

		[Required(ErrorMessage = ReleaseDateRequiredMessage)]
		public string ReleaseDate { get; set; }

		[Required(ErrorMessage = DurationRequiredMessage)]
		[Range(DurationMinValue, DurationMaxValue)]
        public int Duration { get; set; }

		[Required(ErrorMessage = DirectorRequiredMessage)]
		[MinLength(DirectorNameMinLength)]
		[MaxLength(DirectorNameMaxLength)]
		public string Director { get; set; } = null!;

		[Required]
		[MinLength(DescriptionNameMinLength)]
		[MaxLength(DescriptionNameMaxLength)]
		public string Description { get; set; } = null!;
    }
}
