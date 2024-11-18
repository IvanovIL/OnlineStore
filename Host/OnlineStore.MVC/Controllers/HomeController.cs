using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Attributes.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.MVC.Models;
using OnlineStoreApiClients;
using System.Diagnostics;

namespace OnlineStore.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IOnlineStoreApiClient _apiClient;
	
		public HomeController(ILogger<HomeController> logger, 
			IOnlineStoreApiClient apiClient)
		{
			_logger = logger;
			_apiClient = apiClient;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}

		[Authorize (Roles = "Admin")]
		public async Task<IActionResult> Privacy()
        {
			return View();
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
