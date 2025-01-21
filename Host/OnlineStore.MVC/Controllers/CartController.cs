using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.AppServices.Carts.Services;
using OnlineStore.AppServices.Orders.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Order;


namespace OnlineStore.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;

        public CartController(ICartService cartService,
             IProductsService productsService,
             IMapper mapper,
             IOrderServices orderServices)
        {
            _cartService = cartService;
            _productsService = productsService;
            _mapper = mapper;
            _orderServices = orderServices;
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
        public async Task<IActionResult> AllCheckout(OrderDto orderDto, CancellationToken cancellation)
        {
            var cart = await _cartService.GetCartAsync(cancellation);

            orderDto.TotalAmount = cart.TotalAmount;

            await _orderServices.AddOrderAsync(cart, orderDto, cancellation);

            await _productsService.CheckoutAsync(cart, cancellation);

            await _cartService.RemoveAllItemAsync(cancellation);

            return RedirectToAction("getProduct", "Home");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(int ProductId, CancellationToken cancellation)
        {
            var carts = await _cartService.GetCartAsync(cancellation);

            foreach (var cart in carts.Items)
            {
                if (cart.ProductId == ProductId)
                {
                    await _productsService.CheckoutItemAsync(cart, cancellation);
                    break;
                }
            }
            await _cartService.RemoveItemAsync(ProductId, cancellation);
            return RedirectToAction("getProduct", "Home");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> finalCheckout(CancellationToken cancellation)
        {
            var cartItemCount = await _cartService.GetCartAsync(cancellation);

            decimal Total = 0m;

            foreach (var item in cartItemCount.Items)
            {
                Total += item.Price * item.Quantity;
            }


            ViewBag.TotalAmount = Total;
            return View("~/Views/Order/CheckoutOrderView.cshtml");
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
