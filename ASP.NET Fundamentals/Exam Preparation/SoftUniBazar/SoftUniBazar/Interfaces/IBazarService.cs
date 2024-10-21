using SoftUniBazar.Data.Models;
using SoftUniBazar.Models;

namespace SoftUniBazar.Interfaces
{
	public interface IBazarService
	{
		public Task<ICollection<AllAdsAndCartViewModel>> GetAllAdsAsync();
		public Task<ICollection<AllAdsAndCartViewModel>> GetCartAsync(string id);
		public Task<CreateAdViewModel> GetCreateAdAsync();
		public Task<bool> PostCreateAdAsync (CreateAdViewModel model, string id);
		public Task<CreateAdViewModel> GetEditAdAsync(int adId, string currentUserId);
		public Task<CreateAdViewModel> PostEditAdAsync(CreateAdViewModel model, int adId, string id);
		public Task<Ad> FindAdAsync(int id);
		public Task PostAddToCartAsync(string id, Ad ad);
		public Task PostRemoveFromCartAsync(string id, Ad ad);
		public Task<IList<Category>>GetCategoriesAsync();
	}
}
