using GameZone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Diagnostics;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
		{
			//Upon successful Login of an IdentityUser - redirect to the /Game/All
			if (User?.Identity?.IsAuthenticated ?? false)
			{
				return RedirectToAction("All", "Game");
			}
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
