using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationMessages.Cinema;
using static CinemaApp.Common.EntityValidationConstants.Cinema;



namespace CinemaApp.Web.ViewModels.Cinema
{
	public class CinemaIndexViewModel
	{
		public string Id { get; set; } = null!;
		
        public string Name { get; set; } = null!;

		public string Location { get; set; } = null!;
	}
}
