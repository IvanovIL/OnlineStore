using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.MVC.Models;
using System.Diagnostics;

namespace OnlineStore.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IAttributeRepository _attributeRepository;
		public HomeController(ILogger<HomeController> logger, 
			IAttributeRepository attributeRepository)
		{
			_logger = logger;
			_attributeRepository = attributeRepository;
		}
	
		public async Task <IActionResult> Index()
		{
			var attribute = await _attributeRepository.GetAsync(1);
			return View();
		}

		public IActionResult Privacy()
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
