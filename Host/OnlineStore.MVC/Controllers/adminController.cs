using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Categories.Services;
using OnlineStore.AppServices.Orders.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Order;
using OnlineStore.Contracts.Product;


namespace OnlineStore.MVC.Controllers
{
    /// <summary>
    /// Контролер управлением функциями администратора
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class adminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;


        public adminController(ILogger<HomeController> logger,
            IProductsService productService,
            ICategoryService categoryService,
             IOrderServices orderServices,
              IMapper mapper)
        {
            _categoryService = categoryService;
            _logger = logger;
            _productService = productService;
            _orderServices = orderServices;
            _mapper = mapper;
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
        public async Task<IActionResult> saveProduct(ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productService.ChangeProductAsync(productDto, cancellation);

            return RedirectToAction("getProduct", "Home");
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
        public async Task<IActionResult> AddProduct(CancellationToken cancellation)
        {
            var categories = await _categoryService.GetCategoriesAsync(cancellation);

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View("AddProduct");
        }


        [HttpPost]
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
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto, CancellationToken cancellation)
        {
            await _categoryService.AddCategoryAsync(categoryDto, cancellation);

            return RedirectToAction("getProduct", "Home");
        }

        [HttpGet]
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
        public async Task<IActionResult> deleteProduct(int id, CancellationToken cancellation)
        {
            await _productService.DeleteProductAsync(id, cancellation);

            return RedirectToAction("getProduct", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> managementOrder(CancellationToken cancellation)
        {
            var order = await _orderServices.GetOrderAsync(cancellation);

            var OrderStatusDto = await _orderServices.OrderStatusDto();

            ViewBag.OrderStatusDtoS = new SelectList(OrderStatusDto, "Id", "Name");

            return View("managementOrder",order);
        }

        [HttpPost]
        public async Task<IActionResult> changeStatusOrder(int id , OrderDto order, CancellationToken cancellation)
        {
            await _orderServices.changeStatusOrder(id, order.OrderStatusDto.Id, cancellation);

            return RedirectToAction("getProduct", "Home");
        }
    }
}
