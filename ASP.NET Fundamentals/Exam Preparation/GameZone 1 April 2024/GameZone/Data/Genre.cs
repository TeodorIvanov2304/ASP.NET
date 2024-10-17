using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static GameZone.Common.EntityValidationConstants;

namespace GameZone.Data
{
	public class Genre
	{
		[Key]
		[Comment("Genre identifier")]
        public int Id { get; set; }

		[Required]
		[MaxLength(GenreNameMaxLength)]
		[Comment("Genre name")]
		public string Name { get; set; } = null!;

		public IList<Game> Games { get; set; } = new List<Game>();
    }
}