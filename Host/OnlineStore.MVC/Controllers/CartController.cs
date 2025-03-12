using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Carts.Services;
using OnlineStore.AppServices.Orders.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Order;
using OnlineStore.Domain.Entities;

namespace OnlineStore.MVC.Controllers
{
    /// <summary>
    /// Контролер управлением корзиной пользователя
    /// </summary>
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService,
             IProductsService productsService,
             IMapper mapper,
             IOrderServices orderServices,
              UserManager<ApplicationUser> userManager,
              IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _productsService = productsService;
            _mapper = mapper;
            _orderServices = orderServices;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int id, int quantity, CancellationToken cancellation)
        {
            await _cartService.AddProductToCartAsync(id, quantity, cancellation);

            var cartCount = await _cartService.GetCartItemCountAsync(cancellation);

            return RedirectToAction("getProduct", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCartItemCount(CancellationToken cancellation)
        {
            var cartItemCount = await _cartService.GetCartItemCountAsync(cancellation);

            return Json(new { cartItemCount });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(CancellationToken cancellation)
        {
            var cart = await _cartService.GetCartAsync(cancellation);

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellation)
        {
            await _cartService.RemoveItemAsync(productId, cancellation);

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckoutAllOrder(OrderDto orderDto, int productId, CancellationToken cancellation)
        {
            if (productId == 0)
            {
                var cart = await _cartService.GetCartAsync(cancellation);

                orderDto.TotalAmount = cart.TotalAmount;

                await _orderServices.AddOrderAllItemsAsync(cart, orderDto, cancellation);

                await _productsService.CheckoutAsync(cart, cancellation);

                await _cartService.RemoveAllItemAsync(cancellation);

                return RedirectToAction("getProduct", "Home");
            }
            else
            {
                var cartItem = await _cartService.GetCartItemId(productId ,cancellation);

                await _orderServices.AddOrderAsync(cartItem, orderDto, cancellation);

                await _productsService.CheckoutItemAsync(cartItem, cancellation);

                await _cartService.RemoveItemAsync(productId, cancellation);

                return RedirectToAction("getProduct", "Home");
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(int productId, CancellationToken cancellation)
        {
            if (productId == 0)
            {
                var cartItem = await _cartService.GetCartAsync(cancellation);

                decimal Total = 0m;

                foreach (var item in cartItem.Items)
                {
                    Total += item.Price * item.Quantity;
                }
                ViewBag.TotalAmount = Total;

                return View("~/Views/Order/CheckoutOrderView.cshtml");
            }
            else
            {

                var cartItem = await _cartService.GetCartItemId(productId, cancellation);

                ViewBag.TotalAmount = cartItem.Quantity * cartItem.Price;
                ViewBag.productId = productId;

                return View("~/Views/Order/CheckoutOrderView.cshtml");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> removeCart(CancellationToken cancellation)
        {
            await _cartService.RemoveAllItemAsync(cancellation);

            return RedirectToAction("getProduct", "Home");
        }
    }
}
