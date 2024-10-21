using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUniBazar.Data.Models
{
	[PrimaryKey(nameof(BuyerId),nameof(AdId))]
	public class AdBuyer
	{
		[Required]
		[Comment("Buyer identifier")]
		public string BuyerId { get; set; } = null!;

		[Required]
		[Comment("Identity navigation property")]
		[ForeignKey(nameof(BuyerId))]
		public IdentityUser Buyer { get; set; } = null!;

        [Required]
		[Comment("Ad identifier")]
        public int AdId { get; set; }

		[Required]
		[Comment("Ad navigation property")]
		[ForeignKey(nameof(AdId))]
		public Ad Ad { get; set; } = null!;
    }
}
