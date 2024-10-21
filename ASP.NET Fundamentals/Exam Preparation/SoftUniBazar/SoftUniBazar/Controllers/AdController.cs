using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Interfaces;
using SoftUniBazar.Models;

namespace SoftUniBazar.Controllers
{
	[Authorize]
	public class AdController : BaseController
	{	
		private readonly IBazarService _bazarService;

        public AdController(IBazarService bazarService)
        {
            _bazarService = bazarService;
        }

		[HttpGet]
        public async Task<IActionResult> All()
		{
			ICollection<AllAdsAndCartViewModel> model = await  _bazarService.GetAllAdsAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Cart()
		{
			string? currentUserId = GetCurrentUserId();

			ICollection<AllAdsAndCartViewModel> models = await _bazarService.GetCartAsync(currentUserId!);

			return View(models);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			CreateAdViewModel model = await _bazarService.GetCreateAdAsync();

			if (!ModelState.IsValid)
			{
				model.Categories = await _bazarService.GetCategoriesAsync();
				return View(model);
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(CreateAdViewModel model)
		{
			string? currentUserId = GetCurrentUserId();

			bool adExists = await _bazarService.PostCreateAdAsync(model, currentUserId!);

			if (adExists)
			{
				return RedirectToAction(nameof(All));
			}

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			string? currentUserId = GetCurrentUserId();

			CreateAdViewModel model = await _bazarService.GetEditAdAsync(id, currentUserId!);

			if (model == null)
			{
				return RedirectToAction(nameof(All));
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(CreateAdViewModel model, int id)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string? currentUserId = GetCurrentUserId();

			CreateAdViewModel ad = await _bazarService.PostEditAdAsync(model,id, currentUserId!);

			return RedirectToAction(nameof(All));
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(int id)
		{
			Ad? ad = await _bazarService.FindAdAsync(id);

			if (ad is null) 
			{
				return RedirectToAction(nameof(All));
			}

			string? currentUserId = GetCurrentUserId();
			await _bazarService.PostAddToCartAsync(currentUserId!, ad);

			return RedirectToAction(nameof(Cart));
		}

		[HttpPost]
		public async Task<IActionResult> RemoveFromCart(int id)
		{
			Ad? ad = await _bazarService.FindAdAsync(id);

			if (ad is null)
			{
				throw new ArgumentException("Invalid ad");
			}

			string? currentUserId = GetCurrentUserId();

			await _bazarService.PostRemoveFromCartAsync(currentUserId!,ad);

			return RedirectToAction(nameof(All));
		}
	}
}
