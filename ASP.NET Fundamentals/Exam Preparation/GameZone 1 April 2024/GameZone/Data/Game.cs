using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameZone.Common.EntityValidationConstants;


namespace GameZone.Data
{
	public class Game
	{
		[Key]
		[Comment("Unique identifier")]
        public int Id { get; set; }

		[Required]
		[MaxLength(GameTitleMaxLength)]
		[Comment("Game title")]
		public string Title { get; set; } = null!;

		[Required]
		[MaxLength(GameDescriptionMaxLength)]
		[Comment("Game description")]
		public string Description { get; set; } = null!;

		[Comment("The URL of the image")]
        public string? ImageUrl { get; set; }

		[Required]
		[Comment("Identifier of the game Publisher")]
		public string PublisherId { get; set; } = null!;

		[ForeignKey(nameof(PublisherId))]
		public IdentityUser Publisher { get; set; } = null!;

		[Required]
		[Comment("Release date")]
        public DateTime ReleasedOn { get; set; }
		
		[Required]
		[Comment("Game Genre")]
        public int GenreId { get; set; }

		[Required]
		[ForeignKey(nameof(GenreId))]
		public Genre Genre { get; set; } = null!;

		public IList<GamerGame> GamersGames { get; set; } = new List<GamerGame>();

		[Comment("Shows if game is deleted or not")]
        public bool IsDeleted { get; set; } // For soft delete, not required 
    }
}
