using Microsoft.AspNetCore.Mvc;
using MyFirstWebApp.Models;
using System.Diagnostics;

namespace MyFirstWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult Owner()
		{
			var model = new OwnerViewModel()
			{	
				Name = "John Doe",
				Company = "Acme Corporation"
			};

			return View(model);
		}
		public IActionResult About()
		{
			ViewBag.Message = "Your application description page";
			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
