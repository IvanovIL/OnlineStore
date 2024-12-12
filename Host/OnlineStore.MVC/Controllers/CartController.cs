using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using OnlineStore.AppServices.Carts.Services;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using System.Net;

namespace OnlineStore.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;

        private static List<int> productId;


        public CartController(ICartService cartService,
             IProductsService productsService,
             IMapper mapper)
        {
            _cartService = cartService;
            _productsService = productsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity, CancellationToken cancellation)
        {
            await _cartService.AddProductToCartAsync(id, quantity, cancellation);

            var cartCount = await _cartService.GetCartItemCountAsync(cancellation);

            return RedirectToAction("getProduct", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItemCount(CancellationToken cancellation)
        {
            var cartItemCount = await _cartService.GetCartItemCountAsync(cancellation);

            return Json(new { cartItemCount });
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellation)
        {
            var cart = await _cartService.GetCartAsync(cancellation);

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellation)
        {
            await _cartService.RemoveItemAsync(productId, cancellation);

            return RedirectToAction("Index");


        }


        [HttpPost]
        public async Task<IActionResult> AllCheckout(CancellationToken cancellation)
        {
            var cartItemCount = await _cartService.GetCartAsync(cancellation);

            productId = new List<int>();
            decimal Total = 0m;

            foreach (var item in cartItemCount.Items)
            {
                productId.Add(item.ProductId);

                Total += item.Price * item.Quantity;
            }


            ViewBag.productId = productId;
            ViewBag.TotalAmount = Total;
            return View("~/Views/Order/CheckoutOrderView.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int ProductId, string TotalAmount, CancellationToken cancellation)
        {

            decimal Total = Convert.ToDecimal(TotalAmount);
            ViewBag.ProductId = ProductId;
            ViewBag.TotalAmount = Total;

            return View("~/Views/Order/CheckoutOrderView.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> finalCheckout(int ProductId, CancellationToken cancellation)
        {
            if (ProductId != 0)
            {
                await _cartService.RemoveItemAsync(ProductId, cancellation);
            }
            else if (productId.Count != 0)
            {

                for (int i = 0; i < productId.Count; i++)
                {
                    await _cartService.RemoveItemAsync(productId[i], cancellation);
                }
            }
            return RedirectToAction("getProduct", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> removeCart(CancellationToken cancellation)
        {
            await _cartService.RemoveAllItemAsync(cancellation);

            return RedirectToAction("getProduct", "Home");
        }

      
       
    }
}
