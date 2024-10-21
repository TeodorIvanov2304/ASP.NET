using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Data.Models;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{
		protected string? GetCurrentUserId()
		{
			if (User != null)
			{
				return User?.FindFirstValue(ClaimTypes.NameIdentifier);
			}
			
			return string.Empty;
		}

	}
}
