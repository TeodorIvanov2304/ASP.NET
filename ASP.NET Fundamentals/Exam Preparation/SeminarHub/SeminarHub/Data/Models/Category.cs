using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Common.EntityValidationConstants;


namespace SeminarHub.Data.Models
{
	public class Category
	{
		[Key]
		[Comment("Category identifier")]
        public int Id { get; set; }

		[Required]
		[Comment("Category name")]
		[MaxLength(CategoryNameMaxLength)]
		public string Name { get; set; } = null!;
		public virtual ICollection<Seminar> Seminars { get; set; } = new HashSet<Seminar>();
    }
}