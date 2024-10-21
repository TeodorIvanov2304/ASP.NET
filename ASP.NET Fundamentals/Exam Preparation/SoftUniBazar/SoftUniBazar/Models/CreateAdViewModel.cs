using SoftUniBazar.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftUniBazar.Common.EntityValidationConstants;


namespace SoftUniBazar.Models
{
	public class CreateAdViewModel
	{

		[Required]
		[MinLength(AdNameMinLength)]
		[MaxLength(AdNameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MinLength(DescriptionMinLength)]
		[MaxLength(DescriptionMaxLength)]
		public string Description { get; set; } = null!;

		[Required]
		public string ImageUrl { get; set; } = null!;

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int CategoryId { get; set; }

		[Required]
		[ForeignKey(nameof(CategoryId))]
		public ICollection<Category> Categories { get; set; } = new List<Category>();
	}
}
