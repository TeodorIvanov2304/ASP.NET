using CinemaApp.Web.ViewModels.Cinema;
using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationConstants.Movie;

namespace CinemaApp.Web.ViewModels.Movie
{
	public  class AddMovieToCinemaInputModel
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required]
		[MinLength(TitleMinLength)]
		[MaxLength(TitleMaxLength)]
		public string MovieTitle { get; set; } = null!;
        public IList<CinemaCheckBoxItemInputModel> Cinemas { get; set; } = new List<CinemaCheckBoxItemInputModel>();
    }
}
