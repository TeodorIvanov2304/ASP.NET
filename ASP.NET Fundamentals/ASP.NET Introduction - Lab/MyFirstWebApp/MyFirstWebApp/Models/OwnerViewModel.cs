namespace MyFirstWebApp.Models
{
	public class OwnerViewModel
	{
		//required == public  string Name { get; set; } = null!
		public required string Name { get; set; }
        public required string Company { get; set; }
    }
}
