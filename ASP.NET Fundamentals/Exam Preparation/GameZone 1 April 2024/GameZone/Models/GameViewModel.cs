using GameZone.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static GameZone.Common.EntityValidationConstants;


namespace GameZone.Models
{	
	//DTO
	public class GameViewModel
	{
		[Required]
		[MinLength(GameTitleMinLength)]
		[MaxLength(GameTitleMaxLength)]
		public string Title { get; set; } = string.Empty;
        public  string? ImageUrl { get; set; }

		[Required]
		[MinLength(GameDescriptionMinLength)]
		[MaxLength(GameDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

		[Required]
		public string ReleasedOn { get; set; } = DateTime.Today.ToString(GameReleasedOnDateFormat);

		[Required]
        public int GenreId { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
