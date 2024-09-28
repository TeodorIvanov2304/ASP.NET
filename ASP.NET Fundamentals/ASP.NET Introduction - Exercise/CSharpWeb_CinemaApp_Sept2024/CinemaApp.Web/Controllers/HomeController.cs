using System.Diagnostics;            //System namespaces
using Microsoft.AspNetCore.Mvc;      //Third-party namespaces
using CinemaApp.Web.ViewModels;      //Internal project namespaces

namespace CinemaApp.Web.Controllers
{
	public class HomeController : Controller
	{


		public HomeController()
		{
			
		}

		public IActionResult Index()
		{
			//Two ways of transmitting data from Controler to View
			//1. Using ViewData/ViewBag
			//2. Pass ViewModel to the View

			ViewData["Title"] = "Home Page";
			ViewData["Message"] = "Welcome to the Cinema Web App!";
			
			return View();
		}

		
	}
}
