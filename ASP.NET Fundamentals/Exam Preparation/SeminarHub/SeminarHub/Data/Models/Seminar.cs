using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Common.EntityValidationConstants;

namespace SeminarHub.Data.Models
{
	public class Seminar
	{
		[Key]
		[Comment("Seminar identifier")]
        public int Id { get; set; }

		[Required]
		[Comment("Seminar topic name")]
		[MaxLength(TopicMaxLength)]
		public string Topic { get; set; } = null!;

		[Required]
		[Comment("Lecturer name")]
		[MaxLength(LecturerMaxLength)]
		public string Lecturer { get; set; } = null!;

		[Required]
		[Comment("Details for the Seminar")]
		[MaxLength(DetailsMaxLength)]
		public string Details { get; set; } = null!;

		[Required]
		[Comment("Organizer identifier")]
		public string OrganizerId { get; set; } = null!;

		[ForeignKey(nameof(OrganizerId))]
		[Comment("Navigation property")]
		public IdentityUser Organizer { get; set; } = null!;

		[Required]
		[Comment("Date and time of the Seminar")]
        public DateTime DateAndTime { get; set; }

		[Comment("Duration of the Seminar")]
        public int Duration { get; set; }

		[Required]
		[Comment("Category identifier")]
        public int CategoryId { get; set; }

		[Required]
		[Comment("Navigation property")]
		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; } = null!;
		public virtual ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new HashSet<SeminarParticipant>();
    }
}
