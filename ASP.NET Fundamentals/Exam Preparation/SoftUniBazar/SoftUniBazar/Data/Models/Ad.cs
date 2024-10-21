using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftUniBazar.Common.EntityValidationConstants;

namespace SoftUniBazar.Data.Models
{
	public class Ad
	{
		[Key]
        public int Id { get; set; }

		[Required]
		[Comment("Ad name")]
		[MaxLength(AdNameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[Comment("Ad description")]
		[MaxLength(DescriptionMaxLength)]
		public string Description { get; set; } = null!;

		[Required]
		[Comment("Ad price")]
        public decimal Price { get; set; }

		[Required]
		[Comment("Owner identifier")]
		public string OwnerId { get; set; } = null!;
		[Required]
		[Comment("Navigation property")]
		[ForeignKey(nameof(OwnerId))]
        public IdentityUser Owner { get; set; } = null!;

		[Required]
		[Comment("Url of the ad image")]
		public string ImageUrl { get; set; } = null!;

		[Required]
		[Comment("Date created")]
		public DateTime CreatedOn { get; set; }

		[Required]
		[Comment("Category identifier")]
        public int CategoryId { get; set; }

		[Required]
		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; } = null!;
    }
}
