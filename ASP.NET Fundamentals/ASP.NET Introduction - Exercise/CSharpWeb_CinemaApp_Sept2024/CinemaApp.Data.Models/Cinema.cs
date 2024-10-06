namespace CinemaApp.Data.Models
{
	public class Cinema
	{
        public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } 
        public string Location { get; set; }
        public virtual ICollection<CinemaMovie> CinemaMovies { get; set; } = new HashSet<CinemaMovie>();
    }
}
