using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;


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
        [Authorize(Roles = "Admin")]
        public IActionResult adminPanel()
        {
            return View("adminPanelView");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> saveProduct(ShortProductDto productDto, CancellationToken cancellation)
        {

            await _productService.ChangeProductAsync(productDto, cancellation);

            return RedirectToAction("getProduct", "Home");
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(CancellationToken cancellation)
        {
            var categories = await _categoryService.GetCategoriesAsync(cancellation);

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View("AddProduct");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.AddProductAsync(productDto, cancellation);

            return RedirectToAction("getProduct", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            return View("AddCategoryView");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto, CancellationToken cancellation)
        {
            await _categoryService.AddCategoryAsync(categoryDto, cancellation);

            return RedirectToAction("getProduct", "Home");

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteProductList(int pageNumber = 1, CancellationToken cancellation = default)
        {
            var result = await _productService.GetProductsAsync(new PagedRequest
            {
                PageNumber = pageNumber,
                PageSize = 8
            }, cancellation);

            return View("deleteProductView", result);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteProduct(int id, CancellationToken cancellation)
        {
            await _productService.DeleteProductAsync(id, cancellation);

            return RedirectToAction("getProduct", "Home");
        }



    }
}
