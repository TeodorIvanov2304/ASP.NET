using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace CinemaApp.Data.Models
{
	public class ApplicationUser : IdentityUser<Guid>
    {
		public ApplicationUser() 
		{
			this.Id = Guid.NewGuid();
		}

		public ICollection<ApplicationUserMovie> ApplicationUserMovies { get; set; } = new HashSet<ApplicationUserMovie>();
    }
}
