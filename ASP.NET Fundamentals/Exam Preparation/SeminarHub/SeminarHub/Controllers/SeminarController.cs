using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using SQLitePCL;
using System.Globalization;
using System.Security.Claims;
using static SeminarHub.Common.EntityValidationConstants;

namespace SeminarHub.Controllers
{
	[Authorize]
	public class SeminarController : Controller
	{	
		private readonly SeminarHubDbContext _context;

        public SeminarController(SeminarHubDbContext context)
        {
			_context = context;
        }

		[HttpGet]
        public async Task<IActionResult> All()
		{
			var model = await _context.Seminars
				.Select(s => new SeminarInfoViewModel()
				{
					Id = s.Id,
					Topic = s.Topic,
					Lecturer = s.Lecturer,
					Category = s.Category.Name,
					Organizer = s.Organizer.UserName ?? string.Empty,
					DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
				})
				.AsNoTracking()
				.ToListAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new SeminarViewModel();
			model.Categories = await GetCategories();
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Add(SeminarViewModel model)
		{
			string? currentUserId = GetCurrentUserId();

			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategories();
				return View(model);
			}

			return await AddSeminarAsync(model, currentUserId);
		}

		private async Task<IActionResult> AddSeminarAsync(SeminarViewModel model, string? currentUserId)
		{
			DateTime dateAndTime;

			//Check if date is valid

			if (DateTime.TryParseExact(model.DateAndTime, DateAndTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateAndTime) == false)
			{
				ModelState.AddModelError(nameof(model.DateAndTime), "The date format is invalid");
				model.Categories = await GetCategories();
				return View(model);
			}

			var seminar = new Seminar()
			{
				Topic = model.Topic,
				Lecturer = model.Lecturer,
				Details = model.Details,
				DateAndTime = dateAndTime,
				Duration = model.Duration,
				CategoryId = model.CategoryId,
				OrganizerId = currentUserId ?? string.Empty,


			};

			if (await CheckIfSeminarExists(seminar))
			{
				return RedirectToAction(nameof(All));
			}

			await _context.Seminars.AddAsync(seminar);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}

		[HttpPost]
		public async Task<IActionResult> Join(int id)
		{
			Seminar? userSeminar = await _context.Seminars
				.Where(sj => sj.Id == id)
				.Include(sp => sp.SeminarsParticipants)
				.FirstOrDefaultAsync();

			if (userSeminar is null)
			{
				throw new ArgumentException("Invalid seminar");
			}

			var currentUserId = GetCurrentUserId();

			if (userSeminar.SeminarsParticipants.Any(sp => sp.ParticipantId == currentUserId))
			{
				return RedirectToAction(nameof(All));
			}

			userSeminar.SeminarsParticipants.Add(new SeminarParticipant()
			{
				ParticipantId = currentUserId!,
				SeminarId = id
			});

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Joined));

		}

		[HttpGet]
		public async Task<IActionResult> Joined()
		{
			string? currentUserId =  GetCurrentUserId();
			List<JoinedSeminarViewModel> seminars = await _context.Seminars
				.Where(s =>s.SeminarsParticipants.Any(sp => sp.ParticipantId==currentUserId))
				.Select(s => new JoinedSeminarViewModel()
				{	
					Id = s.Id.ToString(),
					Topic = s.Topic,
					Lecturer = s.Lecturer,
					DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
					Organizer = s.Organizer.UserName ?? string.Empty,
				})
				.AsNoTracking()
				.ToListAsync();

			return View(seminars);
		}

		[HttpPost]
		public async Task<IActionResult> Leave(int id)
		{
			var userSeminar = await _context.Seminars
				.Where(s => s.Id == id)
				.Include(g => g.SeminarsParticipants)
				.FirstOrDefaultAsync();

			if (userSeminar is null)
			{
				throw new ArgumentException("Invalid seminar");
			}

			string? currentUserId = GetCurrentUserId();
			SeminarParticipant? seminarParticipant = await _context.SeminarsParticipants
				.FirstOrDefaultAsync(sp => sp.ParticipantId == currentUserId 
				                                            && sp.SeminarId == id);

			if (seminarParticipant is not null) 
			{
				_context.Remove(seminarParticipant);
				await _context.SaveChangesAsync();
			}

			var joinedSeminars = await _context.Seminars
				.Where(s => s.SeminarsParticipants.Any(sp => sp.ParticipantId == currentUserId))
				.Select(s => new JoinedSeminarViewModel()
				{
					Id = s.Id.ToString(),
					Topic = s.Topic,
					Lecturer = s.Lecturer,
					DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
					Organizer = s.Organizer.UserName ?? string.Empty,
				})
				.ToListAsync();


			return View("Joined", joinedSeminars);
		}


		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var model = await _context.Seminars
				.Where(s => s.Id == id)
				.AsNoTracking()
				.Select(s => new DetailsSeminarView()
				{
					Id = s.Id,
					Topic = s.Topic,
					DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
					Duration = s.Duration,
					Lecturer = s.Lecturer,
					Category = s.Category.ToString() ?? string.Empty,
					Organizer = s.Organizer.UserName ?? string.Empty,
					Details = s.Details
				})
				.FirstOrDefaultAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			SeminarViewModel? model = await _context.Seminars

				.Where(s => s.Id == id)
				.Select(s => new SeminarViewModel()
				{
					Topic = s.Topic,
					Lecturer = s.Lecturer,
					Details = s.Details,
					DateAndTime = s.DateAndTime.ToString(DateAndTimeFormat),
					Duration = s.Duration,
					CategoryId = s.CategoryId
				})
				.FirstOrDefaultAsync();

			model.Categories = await GetCategories();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, SeminarViewModel model)
		{
			string currentUserId =  GetCurrentUserId() ?? string.Empty;

			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategories();
				return View(model);
			}

			DateTime dateAndTime;

			//Check if date is valid

			if (DateTime.TryParseExact(model.DateAndTime, DateAndTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateAndTime) == false)
			{
				ModelState.AddModelError(nameof(model.DateAndTime), "The date format is invalid");
				model.Categories = await GetCategories();
				return View(model);
			}

			var seminar = await _context.Seminars.FirstOrDefaultAsync(s => s.Id == id) ?? throw new ArgumentException("Invalid seminar");

			if (seminar.OrganizerId != currentUserId)
			{
				return RedirectToAction(nameof(All));
			}

			seminar.Topic = model.Topic;
			seminar.Lecturer = model.Lecturer;
			seminar.Details = model.Details;
			seminar.DateAndTime = dateAndTime;
			seminar.Duration = model.Duration;
			seminar.CategoryId = model.CategoryId;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _context.Seminars
				.Where(s => s.Id == id)
				.AsNoTracking()
				.Select(s => new DeleteSeminarViewModel()
				{
					Id = id,
					Topic = s.Topic,
					DateAndTime = s.DateAndTime
				})
				.FirstOrDefaultAsync();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed (DeleteSeminarViewModel model)
		{
			var seminar = await _context.Seminars
				.Where(s => s.Id == model.Id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			if (seminar is not null)
			{
				_context.Remove(seminar);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(All));
		}

		private async Task<bool> CheckIfSeminarExists(Seminar seminar)
		{
			return await _context.Seminars.AnyAsync(s => s.Id == seminar.Id);
		}

		private  async Task<List<Category>> GetCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		private string? GetCurrentUserId()
		{
			return User?.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
