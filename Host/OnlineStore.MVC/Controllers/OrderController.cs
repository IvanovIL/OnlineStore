using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Orders.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Order;

namespace OnlineStore.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productService;
        private readonly IOrderServices _orderServices;

        public OrderController(ILogger<HomeController> logger,
            IProductsService productService,
            IOrderServices orderServices)
        {
            _logger = logger;
            _productService = productService;
            _orderServices = orderServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderStatus(CancellationToken cancellation)
        {

            var order = await _orderServices.GetOrderAsync(cancellation);

            return View("OrderStatus", order);
        }
    }
}
