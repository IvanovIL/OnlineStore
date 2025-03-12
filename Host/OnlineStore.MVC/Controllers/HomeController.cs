using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Orders.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.MVC.Models;
using System.Diagnostics;

namespace OnlineStore.MVC.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderServices _orderServices;


        public HomeController(ILogger<HomeController> logger,
            IProductsService productService,
            ICategoryService categoryService,
            IOrderServices orderServices)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _orderServices = orderServices;
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
        public async Task<IActionResult> findProductView(CancellationToken cancellation)
        {
            var categories = await _categoryService.GetCategoriesAsync(cancellation);

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View("findProductView");
        }

        [HttpPost]
        public async Task<IActionResult> findCategory(int CategoryId, int pageNumber = 1, CancellationToken cancellation = default)
        {
            var result = await _categoryService.findCatregoryAsync(CategoryId, new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            return View("getCategoryProduct", result);
        }


        [HttpPost]
        public async Task<IActionResult> FindProduct(ShortProductDto productDto, int pageNumber = 1, CancellationToken cancellation = default)
        {

            var result = await _productService.FindProductAsync(productDto, new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            return View("findProductNameView", result);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellation)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellation)
                ?? throw new ArgumentNullException(nameof(id));
          
            return View("ProductDetails", product);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
