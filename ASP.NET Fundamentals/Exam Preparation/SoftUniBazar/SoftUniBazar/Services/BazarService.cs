using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Interfaces;
using SoftUniBazar.Models;
using static SoftUniBazar.Common.EntityValidationConstants;

namespace SoftUniBazar.Services
{
	public class BazarService : IBazarService
	{
		private readonly BazarDbContext _context;

		public BazarService(BazarDbContext context)
		{
			_context = context;
		}


		public async Task<ICollection<AllAdsAndCartViewModel>> GetAllAdsAsync()
		{
			return await _context.Ads
				.Select(a => new AllAdsAndCartViewModel()
				{
					Name = a.Name,
					ImageUrl = a.ImageUrl,
					Category = a.Category.Name,
					CreatedOn = a.CreatedOn.ToString(CreatedOnDateFormat),
					Description = a.Description,
					Price = a.Price,
					Owner = a.Owner.UserName ?? string.Empty,
					Id = a.Id
				})
				.ToListAsync();
		}

		public async Task<ICollection<AllAdsAndCartViewModel>> GetCartAsync(string id)
		{
			return await _context.AdsBuyers
				.Where(ab => ab.BuyerId == id)
				.Select(ab => new AllAdsAndCartViewModel()
				{
					Id = ab.AdId,
					Name = ab.Buyer.UserName ?? string.Empty,
					ImageUrl = ab.Ad.ImageUrl,
					Category = ab.Ad.Category.Name,
					CreatedOn = ab.Ad.CreatedOn.ToString(CreatedOnDateFormat),
					Description = ab.Ad.Description,
					Price = ab.Ad.Price,
					Owner = ab.Ad.Owner.UserName ?? string.Empty,
				})
				.ToListAsync();

		}

		public async Task<IList<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<CreateAdViewModel> GetCreateAdAsync()
		{
			CreateAdViewModel model = new CreateAdViewModel();
			model.Categories = await GetCategoriesAsync();
			return model;
		}

		public async Task<bool> PostCreateAdAsync(CreateAdViewModel model, string id)
		{
			if (_context.Ads.Any(a => a.OwnerId == id && a.Name == model.Name))
			{
				return true;
			}

			Ad ad = new Ad()
			{
				OwnerId = id,
				Name = model.Name,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				Price = model.Price,
				CategoryId = model.CategoryId,
				CreatedOn = DateTime.Now
			};

			await _context.Ads.AddAsync(ad);
			await _context.SaveChangesAsync();

			return false;
		}

		public async Task<CreateAdViewModel> GetEditAdAsync(int adId, string currentUserId)
		{
			CreateAdViewModel? model = await _context.Ads
						.Where(a => a.Id == adId && a.OwnerId == currentUserId)
						.Select(a => new CreateAdViewModel()
						{
							Name = a.Name,
							Description = a.Description,
							ImageUrl = a.ImageUrl,
							Price = a.Price,
							CategoryId = a.CategoryId,
						})
						.FirstOrDefaultAsync();

			if (model == null)
			{
				return null!;
			}

			model.Categories = await GetCategoriesAsync();

			return model;
		}

		public async Task<CreateAdViewModel> PostEditAdAsync(CreateAdViewModel model, int adId, string currentUserId)
		{
			Ad? ad = await _context.Ads.FindAsync(adId);

			if (ad == null)
			{
				throw new ArgumentException("Ad not found!");
			}

			if (ad.OwnerId != currentUserId)
			{
				throw new ArgumentException("Invalid operation! You are not the owner.");
			}

			ad.Name = model.Name;
			ad.Description = model.Description;
			ad.ImageUrl = model.ImageUrl;
			ad.Price = model.Price;

			ad.CategoryId = model.CategoryId;

			await _context.SaveChangesAsync();

			return model;
		}

		public async Task<Ad?> FindAdAsync(int id)
		{
			return await _context.Ads
								   .Where(ad => ad.Id == id)
								   .Include(ab => ab.Category)
								   .FirstOrDefaultAsync();

		}

		public async Task PostAddToCartAsync(string id, Ad ad)
		{
			if (!await _context.AdsBuyers.AnyAsync(ab => ab.BuyerId == id && ab.AdId == ad.Id))
			{
				AdBuyer adsBuyers = new AdBuyer()
				{
					AdId = ad.Id,
					BuyerId = id
				};

				await _context.AdsBuyers.AddAsync(adsBuyers);
				await _context.SaveChangesAsync();
			}
		}

		public async Task PostRemoveFromCartAsync(string id, Ad ad)
		{
			AdBuyer? userAd = await _context.AdsBuyers
				.FirstOrDefaultAsync(ua => ua.BuyerId == id && ua.AdId == ad.Id);

			if (userAd is not null)
			{
				_context.AdsBuyers.Remove(userAd);
				await _context.SaveChangesAsync();
			}
		}

	}
}
