namespace SoftUniBazar.Models
{
	public class AllAdsAndCartViewModel
	{
		public required int Id { get; set; } 

		public required string Name { get; set; } 

		public required string ImageUrl { get; set; } 

		public required string CreatedOn { get; set; }

		public required string Category { get; set; } 

		public required string Description { get; set; } 

		public required decimal Price { get; set; }

		public required string Owner { get; set; }
	}
}
