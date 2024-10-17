using GameZone.Data;
using GameZone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Security.Policy;
using static GameZone.Common.EntityValidationConstants;

namespace GameZone.Controllers
{
	[Authorize] // All actions will require Log in
	public class GameController : Controller
	{
		private readonly GameZoneDbContext _dbContext;
		private readonly UserManager<IdentityUser> _userManager;

		public GameController(GameZoneDbContext dbContext, UserManager<IdentityUser> userManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> All()
		{
			var model = await _dbContext.Games
							  .Where(g => g.IsDeleted == false)
							  .Select(g => new GameInfoViewModel()
							  {
								  Id = g.Id,
								  Genre = g.Genre.Name,
								  ImageUrl = g.ImageUrl,
								  //If UserName == null
								  Publisher = g.Publisher.UserName ?? string.Empty,
								  ReleasedOn = g.ReleasedOn.ToString(GameReleasedOnDateFormat),
								  Title = g.Title,

							  })
							  .AsNoTracking()
							  .ToListAsync();

			return View(model);
		}

		//Get the Add form
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new GameViewModel();
			model.Genres = await GetGenres();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(GameViewModel model)
		{

			//var userId = _userManager.GetUserId(User);
			var userId = GetCurrentUserId();

			if (!ModelState.IsValid)
			{
				model.Genres = await GetGenres();
				return View(model);
			}

			DateTime releasedOn;

			//Check if date is valid

			if (DateTime.TryParseExact(model.ReleasedOn, GameReleasedOnDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out releasedOn) == false)
			{
				ModelState.AddModelError(nameof(model.ReleasedOn), "Invalid date format");
				model.Genres = await GetGenres();
				return View(model);
			}

			Game game = new Game()
			{
				Description = model.Description,
				GenreId = model.GenreId,
				ImageUrl = model.ImageUrl,
				PublisherId = userId ?? string.Empty,
				Title = model.Title,
				ReleasedOn = releasedOn
			};

			await _dbContext.Games.AddAsync(game);
			await _dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			GameViewModel? model = await _dbContext.Games
				.Where(g => g.Id == id)
				.Where(g => g.IsDeleted == false)
				.AsNoTracking()
				.Select(g => new GameViewModel()
				{
					Description = g.Description,
					GenreId = g.GenreId,
					ImageUrl = g.ImageUrl,
					ReleasedOn = g.ReleasedOn.ToString(GameReleasedOnDateFormat),
					Title = g.Title,
				})
				.FirstOrDefaultAsync();

			model.Genres = await GetGenres();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(GameViewModel model, int id)
		{

			string currentUserId = GetCurrentUserId() ?? string.Empty;

			if (!ModelState.IsValid)
			{
				model.Genres = await GetGenres();
				return View(model);
			}

			DateTime releasedOn;

			//Check if date is valid

			if (DateTime.TryParseExact(model.ReleasedOn, GameReleasedOnDateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out releasedOn) == false)
			{
				ModelState.AddModelError(nameof(model.ReleasedOn), "Invalid date format");
				model.Genres = await GetGenres();
				return View(model);
			}

			Game? entity = await _dbContext.Games.FindAsync(id);

			if (entity is null || entity.IsDeleted)
			{
				throw new ArgumentException("Invalid id");
			}


			if (entity.PublisherId != currentUserId)
			{
				return RedirectToAction(nameof(All));
			}

			entity.Description = model.Description;
			entity.GenreId = model.GenreId;
			entity.ImageUrl = model.ImageUrl;
			entity.PublisherId = currentUserId ?? string.Empty;
			entity.Title = model.Title;
			entity.ReleasedOn = releasedOn;

			await _dbContext.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> MyZone()
		{
			string currentUserId = GetCurrentUserId() ?? string.Empty;

			var model = await _dbContext.Games
							  .Where(g => g.IsDeleted == false)
							  .Where(g => g.GamersGames.Any(gg => gg.GamerId == currentUserId))
							  .Select(g => new GameInfoViewModel()
							  {
								  Id = g.Id,
								  Genre = g.Genre.Name,
								  ImageUrl = g.ImageUrl,
								  Publisher = g.Publisher.UserName ?? string.Empty,
								  ReleasedOn = g.ReleasedOn.ToString(GameReleasedOnDateFormat),
								  Title = g.Title,

							  })
							  .AsNoTracking()
							  .ToListAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> AddToMyZone(int id)
		{
			Game? entity = await _dbContext.Games
						   .Where(g => g.Id == id)
						   .Include(g => g.GamersGames)
						   .FirstOrDefaultAsync();

			if (entity is null || entity.IsDeleted)
			{
				throw new ArgumentException("Invalid id");
			}

			string currentUserId = GetCurrentUserId() ?? string.Empty;

			if (entity.GamersGames.Any(gg => gg.GamerId == currentUserId))
			{
				return RedirectToAction(nameof(All));
			}

			entity.GamersGames.Add(new GamerGame()
			{
				GameId = entity.Id,
				GamerId = currentUserId
			});

			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(MyZone));
		}

		[HttpGet]
		public async Task<IActionResult> StrikeOut(int id)
		{
			Game? entity = await _dbContext.Games
						   .Where(g => g.Id == id)
						   .Include(g => g.GamersGames)
						   .FirstOrDefaultAsync();

			if (entity is null || entity.IsDeleted)
			{
				throw new ArgumentException("Invalid id");
			}

			string currentUserId = GetCurrentUserId() ?? string.Empty;
			GamerGame? gamerGame = entity.GamersGames.FirstOrDefault(gg => gg.GamerId == currentUserId);

			if (gamerGame is not null)
			{
				entity.GamersGames.Remove(gamerGame);

				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction(nameof(MyZone));
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var model = await _dbContext.Games
				.Where(g => g.Id == id)
				.Where(g => g.IsDeleted == false)
				.AsNoTracking()
				.Select(g => new DetailsGameInfoModel()
				{
					Id = g.Id,
					Description = g.Description,
					Genre = g.Genre.Name,
					ImageUrl = g.ImageUrl,
					ReleasedOn = g.ReleasedOn.ToString(GameReleasedOnDateFormat),
					Title = g.Title,
					Publisher = g.Publisher.UserName ?? string.Empty
				})
				.FirstOrDefaultAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _dbContext.Games
				.Where(g => g.Id == id)
				.Where(g => g.IsDeleted == false)
				.AsNoTracking()
				.Select(g => new DeleteViewModel()
				{
					Id = g.Id,
					Title = g.Title,
					Publisher = g.Publisher.UserName ?? string.Empty
				})
				.FirstOrDefaultAsync();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(DeleteViewModel deleteModel)
		{
			Game? game = await _dbContext.Games
				.Where(g => g.Id == deleteModel.Id)
				.Where(g => g.IsDeleted == false)
				.FirstOrDefaultAsync();

			if (game is not null)
			{
				//Soft delete
				game.IsDeleted = true;

				await _dbContext.SaveChangesAsync();

			}

			return RedirectToAction(nameof(All));
		}
		private string? GetCurrentUserId()
		{
			return User?.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		private async Task<List<Genre>> GetGenres()
		{
			return await _dbContext.Genres.ToListAsync();
		}
	}
}
