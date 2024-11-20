using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.MVC.Models;
using System.Diagnostics;

namespace OnlineStore.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductsService _productService;
		private readonly ICategoryService _categoryService;


        public HomeController(ILogger<HomeController> logger, 
            IProductsService productService,
            ICategoryService categoryService)
		{
			_logger = logger;
			_productService = productService;
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index()
		{

            return View();
		}

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View("AddProduct");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.AddProductAsync(productDto, cancellation);

            return Ok();
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
