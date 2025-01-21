using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Enums;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Carts.Services
{
    /// <summary>
    /// Сервис по работе с корзиной
    /// </summary>
    public sealed class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductsService _productsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartService(ICartRepository cartRepository,
            IProductsService productsService,
            UserManager<ApplicationUser> userManager,
            IDataTimeProvider dataTimeProvider,
            IHttpContextAccessor httpContextAccessor,
              IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productsService = productsService;
            _userManager = userManager;
            _dataTimeProvider = dataTimeProvider;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task AddProductToCartAsync(int productId,int quantity ,CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingCart = await _cartRepository.GetCartByUserAsync(user.Id, cancellation);

            if (existingCart == null)
            {
                existingCart = new Cart
                {
                    UserId = user.Id,
                    Created = _dataTimeProvider.UtcNow,
                    StatusId = (int)CartStatusEnum.New,
                };

                AddProductToCart(existingCart, productId, quantity);

                await _cartRepository.AddAsync(existingCart, cancellation);
            }
            else
            {
                
                AddProductToCart(existingCart, productId, quantity);
                existingCart.Updated = _dataTimeProvider.UtcNow;
                await _cartRepository.UpdateAsync(existingCart, cancellation);
            }

        }
        private static void AddProductToCart(Cart cart, int productId,int quantity)
        {
            var productInCart = cart.Products.FirstOrDefault(p => p.ProductId == productId);

            if (productInCart != null)
            {
                productInCart.Quantity += quantity;
            }
            else
            {
                cart.Products.Add(new CartProduct
                {

                    Cart = cart,
                    Quantity = quantity,
                    ProductId = productId,

                });

            }
        }
        public async Task<CartDto> GetCartAsync(CancellationToken cancellation)
        {
            var Cart = await GetCurrentUserCartAsync(cancellation);

            if (Cart == null)
            {
                return new CartDto
                {
                    Items = [],
                    TotalAmount = 0
                };
            }

            return await GetCartItemsAsync(Cart,cancellation);
        }

        public async Task<int?> GetCartItemCountAsync(CancellationToken cancellation)
        {
            var exictingCart = await GetCurrentUserCartAsync(cancellation);
            return exictingCart?.Products?.Sum(x => x.Quantity) ?? 0;
        }
        
        private async Task<Cart> GetCurrentUserCartAsync(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if(user == null)
            {
                return null;
            }

            return await _cartRepository.GetCartByUserAsync(user.Id , cancellation);
        }

        private async Task<CartDto> GetCartItemsAsync(Cart cart, CancellationToken cancellation)
        {
            var products = await _productsService.GetProductsAsync(new PagedRequest(), cancellation);

            var cartItems = new List<CartItemDto>(products.Result.Count);

            var totalAmount = 0m;

            foreach (var productInCart in cart.Products)
            {
                var product = products.Result.FirstOrDefault(x => x.Id == productInCart.ProductId);
                cartItems.Add(new CartItemDto
                {
                    Price = product.Price,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = productInCart.Quantity
                });

                totalAmount += product.Price * productInCart.Quantity;
            }

            return new CartDto
            {
                Items = cartItems,
                TotalAmount = Math.Round(totalAmount, 2)
            };
        }

        public async Task RemoveItemAsync(int productId, CancellationToken cancellation)
        {
            var cart = await GetCurrentUserCartAsync(cancellation)
            ?? throw new InvalidOperationException("Не найдена корзина текущего пользователя");

            var productInCart = cart.Products.FirstOrDefault(x => x.ProductId == productId) 
                ?? throw new InvalidOperationException("Не найден товар в корзине текущего пользователя для удаления");

            cart.Products.Remove(productInCart);


            await _cartRepository.UpdateAsync(cart, cancellation);
        }

        public async Task RemoveAllItemAsync (CancellationToken cancellation)
        {
            var cart = await GetCurrentUserCartAsync(cancellation)
            ?? throw new InvalidOperationException("Не найдена корзина текущего пользователя");

            cart.Products.Clear();

            await _cartRepository.UpdateAsync(cart, cancellation);
        }

      

    }
}
