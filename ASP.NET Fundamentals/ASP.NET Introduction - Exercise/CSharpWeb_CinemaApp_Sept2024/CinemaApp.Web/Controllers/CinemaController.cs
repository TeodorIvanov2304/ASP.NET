using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Cinema;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{	
	//DI .NET 8 
	public class CinemaController(CinemaDbContext dbContext) : BaseController
	{
		[HttpGet]
        public async Task<IActionResult> Index()
		{
			IEnumerable<CinemaIndexViewModel> cinemas = await dbContext
				.Cinemas
				.Select(c => new CinemaIndexViewModel()
				{
					Id = c.Id.ToString(),
					Name = c.Name,
					Location = c.Location,
				})
				.OrderBy(c=>c.Location)
				.ToListAsync();

			return View(cinemas); 
		}

		[HttpGet]
		public IActionResult Create()
		{
			
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AddCinemaFormModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return View(model);
			}

			Cinema cinema = new Cinema()
			{	
				Name = model.Name,
				Location = model.Location
			};

			await dbContext.Cinemas.AddAsync(cinema);
			await dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(string? id)
		{
			Guid cinemaGuid = Guid.Empty;
			bool isValid =  IsGuidValid(id, ref cinemaGuid);

			if (!isValid)
			
			{
				return this.RedirectToAction(nameof(Index));
			}

			Cinema? cinema = await dbContext.Cinemas
				.Include(c => c.CinemaMovies)
				.ThenInclude(cm => cm.Movie)
				.FirstOrDefaultAsync(c => c.Id == cinemaGuid);

			//Invalid(non-existing) Guid in the URL
			if (cinema is null)
			{
				return this.RedirectToAction(nameof(Index));
			}

			CinemaDetailsViewModel viewModel = new CinemaDetailsViewModel()
			
			{
				Name = cinema.Name,
				Location = cinema.Location,
				Movies = cinema.CinemaMovies.Select(cm => new CinemaMovieViewModel()
				{
					Title = cm.Movie.Title,
					Duration = cm.Movie.Duration
				})
				.ToArray()
			};

			return this.View(viewModel);
		}

    }
}
