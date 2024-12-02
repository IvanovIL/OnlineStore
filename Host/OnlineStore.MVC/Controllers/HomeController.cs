using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Common.NotificationServices;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Notifications;
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

        [HttpGet]
		public async Task<IActionResult> personalAccount()
        {
            return View("personalAccount");
		}


        public async Task<IActionResult> Index()
        {

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> getProduct(int pageNumber = 1, CancellationToken cancellation = default)
        {
            var result = await _productService.GetProductsAsync(new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);



            return View(result);
        }


        public async Task<IActionResult> Details(int id, CancellationToken cancellation)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellation);
            if (product == null)
            {
                return NotFound();
            }

            return View("ProductDetails", product);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View("AddProduct");
        }


        public async Task<IActionResult> AddProduct(CancellationToken cancellation)
        {
            var categories = await _categoryService.GetCategoriesAsync(cancellation);

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View("getProduct");
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.AddProductAsync(productDto, cancellation);

            return RedirectToAction("getProduct");
        }

        [HttpGet]
        public async Task<IActionResult> deleteProduct()
        {
            return View("deleteProductView");
        }


        [HttpPost]
        public async Task<IActionResult> deleteProduct(int id, CancellationToken cancellation)
        {
            await _productService.DeleteProductAsync(id, cancellation);

            return RedirectToAction("getProduct");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
