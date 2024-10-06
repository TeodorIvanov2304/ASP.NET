using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Movie;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static CinemaApp.Common.EntityValidationConstants.Movie;
using System.Runtime.CompilerServices;
namespace CinemaApp.Web.Controllers
{
	public class MovieController : BaseController
	{
		private readonly CinemaDbContext _dbContext;

		//Dependency Injection
        public MovieController(CinemaDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

		[HttpGet]
        public async Task<IActionResult> Index()
		{	
			IEnumerable<Movie> movies = await this._dbContext.Movies.ToListAsync();

			return View(movies);
		}

		[HttpGet]
		public  IActionResult Create()
		{	

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AddMovieInputModel inputModel)
		{

			bool isReleaseDateValid = DateTime.TryParseExact(inputModel.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

			if (!isReleaseDateValid)
			{	
				//ModelState becomes invalid => isValid == false
				 this.ModelState.AddModelError(nameof(inputModel.ReleaseDate), String.Format($"The Release Date must be in the following format: {0}", ReleaseDateFormat));
				return this.View(inputModel);
			}

			//! or not ??
			if (!this.ModelState.IsValid)
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

			await _dbContext.Movies.AddAsync(movie);
			await _dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(string id)
		{
			Guid movieGuid = Guid.Empty;

			bool isGuidVali = this.IsGuidValid(id, ref movieGuid);

			if (!isGuidVali)
			{	
				//Invalid id format
				return RedirectToAction(nameof(Index));
			}
			
			Movie? movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movieGuid);

			if (movie is null)
			{	
				//Non-existing movie Guid
				return RedirectToAction(nameof(Index));
			}

			return this.View(movie);
		}

		[HttpGet]
		public async Task<IActionResult> AddToProgram(string? id)
		{
			Guid movieGuid = Guid.Empty;
			bool isGuidValid = this.IsGuidValid(id, ref movieGuid);

			if (!isGuidValid)
			{
				return RedirectToAction(nameof(Index));
			}

			Movie? movie = await this._dbContext
				.Movies
				.FirstOrDefaultAsync(m => m.Id == movieGuid);

			if (movie is null)
			{
				return this.RedirectToAction(nameof(Index));
			}

			AddMovieToCinemaInputModel viewModel = new AddMovieToCinemaInputModel()
			{
				Id = id!,
				MovieTitle =  movie.Title,
				Cinemas = await this._dbContext.Cinemas
							  .Include(c => c.CinemaMovies)
							  .ThenInclude(cm => cm.Movie)
							  .Select(c => new CinemaCheckBoxItemInputModel()
							  {
								  Id = c.Id.ToString(),
								  Name = c.Name,
								  Location = c.Location,
								  IsSelected = c.CinemaMovies.Any(cm => cm.Movie.Id == movieGuid)
							  })
							  .ToArrayAsync()
			};

			return this.View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> AddToProgram(AddMovieToCinemaInputModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return View(model);
			}

			Guid movieGuid = Guid.Empty;
			bool isGuidValid = this.IsGuidValid(model.Id, ref movieGuid);

			if (!isGuidValid)
			{
				return RedirectToAction(nameof(Index));
			}

			Movie? movie = await this._dbContext.Movies
				.FirstOrDefaultAsync(m=> m.Id == movieGuid);

			if (movie is null)
			{
				return RedirectToAction(nameof(Index));
			}

			ICollection<CinemaMovie> entitiesToAdd = new List<CinemaMovie>();

			foreach (CinemaCheckBoxItemInputModel cinemaInputModel in model.Cinemas)
			{
				Guid cinemaGuid = Guid.Empty;
				bool isCinemaGuidValid = this.IsGuidValid(cinemaInputModel.Id, ref cinemaGuid);

				if (!isCinemaGuidValid)
				{
					this.ModelState.AddModelError(string.Empty, "Invalid cinema selected!");
					return this.View(model);
				}

				Cinema? cinema = await this._dbContext.Cinemas
									.FirstOrDefaultAsync(c => c.Id == cinemaGuid);

				if (cinema is null)
				{
					this.ModelState.AddModelError(string.Empty, "Invalid cinema selected!");
					return this.View(model);
				}

				CinemaMovie? cinemaMovie = await this._dbContext.CinemasMovies
						.FirstOrDefaultAsync(cm => cm.MovieId == movieGuid &&
												   cm.CinemaId == cinemaGuid);

				if (cinemaInputModel.IsSelected)
				{
					if (cinemaMovie is null)
					{
						entitiesToAdd.Add(new CinemaMovie()
						{
							Cinema = cinema,
							Movie = movie
						});
					}
					else
					{
						cinemaMovie.IsDeleted = false;
					}
				}
				else
				{
					
					if (cinemaMovie is not null)
					{
						cinemaMovie.IsDeleted = true;
					}
				}

				await this._dbContext.SaveChangesAsync();
			}

			await this._dbContext.AddRangeAsync(entitiesToAdd);
			await this._dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index),"Cinema");
		}


	}
}
