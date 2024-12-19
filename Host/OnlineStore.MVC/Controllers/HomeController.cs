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

        [HttpGet]
        public async Task<IActionResult> personalAccount()
        {
            return View("personalAccount");
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

        [HttpGet]
        public IActionResult findCategoryProductView()
        {
            return View("findCategoryProductView");
        }

       
        [HttpPost]
        public async Task<IActionResult> FindProduct(string name, int pageNumber = 1, CancellationToken cancellation = default)
        {
            var result = await _productService.FindProductAsync(name,new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            return View("findProductNameView", result);
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
