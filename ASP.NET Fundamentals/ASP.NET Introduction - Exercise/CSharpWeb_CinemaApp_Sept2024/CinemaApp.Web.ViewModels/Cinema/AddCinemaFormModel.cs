using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationMessages.Cinema;
using static CinemaApp.Common.EntityValidationConstants.Cinema;

namespace CinemaApp.Web.ViewModels.Cinema
{
	public class AddCinemaFormModel
	{
		[Required(ErrorMessage = NameRequiredMessage)]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = LocationRequiredMessage)]
		[MinLength(LocationMinLength)]
		[MaxLength(LocationMaxLength)]
		public string Location { get; set; } = null!;
    }
}
