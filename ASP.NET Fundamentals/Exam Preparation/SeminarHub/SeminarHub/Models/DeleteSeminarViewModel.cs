namespace SeminarHub.Models
{
	public class DeleteSeminarViewModel
	{
		public int Id { get; set; }
        public required string Topic { get; set; }
        public required DateTime DateAndTime { get; set; }
    }
}
