using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data.Models
{
	[PrimaryKey(nameof(SeminarId),nameof(ParticipantId))]
	public class SeminarParticipant
	{
		[Comment("Seminar identifier")]
        public int SeminarId { get; set; }

		[ForeignKey(nameof(SeminarId))]
		public Seminar Seminar { get; set; } = null!;

		[Comment("Participant identifier")]
		public string ParticipantId { get; set; } = null!;

		[ForeignKey(nameof(ParticipantId))]
		public IdentityUser Participant { get; set; } = null!;
    }
}