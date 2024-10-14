using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Common.EntityValidationConstants.Movie;

namespace CinemaApp.Web.Controllers
{
	//[Authorize]
	//DI in the primary constructor
	public class WatchlistController(CinemaDbContext dbContext, UserManager<ApplicationUser> userManager) : BaseController
	{
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			//ApplicationUser? user = await userManager.GetUserAsync(User);

			string? userId =  userManager.GetUserId(User)!;

			IEnumerable<ApplicationUserWatchListViewModel> watchlist =  await dbContext.UserMovies
				.Include(um => um.Movie)
				.Where(um => um.ApplicationUserId.ToString().ToLower() == userId.ToLower())
				.Select(um => new ApplicationUserWatchListViewModel()
				{
					MovieId = um.MovieId.ToString(),
					Title = um.Movie.Title,
					Genre = um.Movie.Genre,
					ReleaseDate = um.Movie.ReleaseDate.ToString(ReleaseDateFormat),
					ImageUrl = um.Movie.ImageUrl
				})
				.ToListAsync();


			return View(watchlist);
		}

		[HttpPost]
		public async Task<IActionResult> AddToWatchList(string movieId)
		{	
			Guid movieGuid = Guid.Empty;
			if (!IsGuidValid(movieId, ref movieGuid))
			{
				return RedirectToAction("Index","Movie");
			}

			Movie? movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movieGuid);

			if (movie is null)
			{
				return RedirectToAction("Index", "Movie");
			}

			Guid userGuid = Guid.Parse(userManager.GetUserId(User)!);
			bool addedToWatchListAlready = await dbContext.UserMovies.AnyAsync(um => um.MovieId == movieGuid);

			if (!addedToWatchListAlready)
			{
				ApplicationUserMovie newUserMovie = new()
				{
					ApplicationUserId = userGuid,
					MovieId = movieGuid
				};

				await dbContext.UserMovies.AddAsync(newUserMovie);
				await dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> RemoveFromWatchlist(string? movieId)
		{
			Guid movieGuid = Guid.Empty;
			if (!IsGuidValid(movieId, ref movieGuid))
			{
				return RedirectToAction("Index", "Movie");
			}

			Movie? movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movieGuid);

			if (movie is null)
			{
				return RedirectToAction("Index", "Movie");
			}

			Guid userGuid = Guid.Parse(userManager.GetUserId(User)!);
			ApplicationUserMovie? applicationUserMovie = await dbContext.UserMovies.FirstOrDefaultAsync(um => um.MovieId == movieGuid);

			if (applicationUserMovie is not null)
			{
				dbContext.UserMovies.Remove(applicationUserMovie);
				await dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
