using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;


namespace OnlineStore.MVC.Controllers
{
    public class adminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productService;
        private readonly ICategoryService _categoryService;

        public adminController(ILogger<HomeController> logger,
            IProductsService productService,
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _logger = logger;
            _productService = productService;
        }


        [HttpGet]
        public IActionResult adminPanel()
        {
            return View("adminPanelView");
        }

        [HttpGet]
        public async Task<IActionResult> getChangeProduct(int pageNumber = 1, CancellationToken cancellation = default)
        {
            var result = await _productService.GetProductsAsync(new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> saveProduct(ShortProductDto productDto,CancellationToken cancellation)
        {
           
            await _productService.ChangeProductAsync(productDto, cancellation);

            return RedirectToAction("getProduct","Home");
        }

        public async Task<IActionResult> DetailsChangeProduct(int id, CancellationToken cancellation)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellation);
            if (product == null)
            {
                return NotFound();
            }

            return View("changeProductView", product);
        }

        [HttpGet]
        public async Task<IActionResult> getDeleteProduct(int pageNumber = 1, CancellationToken cancellation = default)
        {
           
            var result = await _productService.GetProductsAsync(new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            ViewBag.IsDeleted = true;
            
            return View("~/Views/Home/getProduct.cshtml", result);
        }


    }
}
