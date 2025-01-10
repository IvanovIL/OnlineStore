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
            var order = await _orderServices.GetOrdersAllAsync(cancellation);

            return View("~/Views/Order/OrderStatus.cshtml", order);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getOrders(CancellationToken cancellation)
        {
            var order = await _orderServices.GetOrderAsync(cancellation);

            return View("~/Views/Home/personalAccount.cshtml", order.ToList());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getOrder(int Id, CancellationToken cancellation)
        {
            var order = await _orderServices.GetOrderIdAsync(Id, cancellation);

            ViewBag.OrderId = Id;
            return View("~/Views/Order/Order.cshtml", order);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrderProduct(int OrderId, int ProductId, CancellationToken cancellation)
        {
            await _orderServices.CancelOrderProduct(OrderId, ProductId, cancellation);

            return RedirectToAction("getOrders");

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int OrderId, CancellationToken cancellation)
        {
            await _orderServices.CancelOrderAsync(OrderId, cancellation);

            return RedirectToAction("getOrders");

        }




    }
}
