using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Models;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Common.EntityValidationConstants;

namespace SeminarHub.Models
{
	public class SeminarViewModel
	{
		[Required]
		[MinLength(TopicMinLength)]
		[MaxLength(TopicMaxLength)]
		public string Topic { get; set; } = null!;

		[Required]
		[MinLength(LecturerMinLength)]
		[MaxLength(LecturerMaxLength)]
		public string Lecturer { get; set; } = null!;

		[Required]
		[MinLength(DetailsMinLength)]
		[MaxLength(DetailsMaxLength)]
		public string Details { get; set; } = null!;

		[Required]
		public string DateAndTime { get; set; } = DateTime.Today.ToString(DateAndTimeFormat);

		[Required]
		[Range(DurationMinValue,DurationMaxValue)]
		public int Duration { get; set; }

		[Required]
		public int CategoryId { get; set; }

		public IList<Category> Categories { get; set; } = new List<Category>();
	}
}
