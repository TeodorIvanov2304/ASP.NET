using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CinemaApp.Web.Controllers
{
	public class MovieController : Controller
	{
		private readonly CinemaDbContext _dbContext;

		//Dependency Injection
        public MovieController(CinemaDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

		[HttpGet]
        public IActionResult Index()
		{	
			IEnumerable<Movie> movies = this._dbContext.Movies.ToList();

			return View(movies);
		}

		[HttpGet]
		public IActionResult Create()
		{	

			return View();
		}

		[HttpPost]
		public IActionResult Create(AddMovieInputModel inputModel)
		{

			

			bool isReleaseDateValid = DateTime.TryParseExact(inputModel.ReleaseDate, "MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

			if (!isReleaseDateValid)
			{
				this.ModelState.AddModelError(nameof(inputModel.ReleaseDate), "The Release Date must be in the following format: MMMM yyyy");
				return this.View(inputModel);
			}

			if (this.ModelState.IsValid)
			{
				//Render the same form with user entered values + model errors
				return View(inputModel);
			}

			Movie movie = new Movie() 
			{
				Title = inputModel.Title,
				Genre = inputModel.Genre,
				ReleaseDate = releaseDate,
				Director = inputModel.Director,
				Duration = inputModel.Duration,
				Description = inputModel.Description
			};

			_dbContext.Movies.Add(movie);
			_dbContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult Details(string id)
		{
			bool isValidId = Guid.TryParse(id, out Guid guidId);

			if (!isValidId)
			{	
				//Invalid id format
				return RedirectToAction(nameof(Index));
			}
			
			Movie? movie = _dbContext.Movies.FirstOrDefault(m => m.Id == guidId);

			if (movie is null)
			{	
				//Non-existing movie Guid
				return RedirectToAction(nameof(Index));
			}

			return this.View(movie);
		}
	}
}
