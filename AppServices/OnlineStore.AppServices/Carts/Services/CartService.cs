using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Enums;
using OnlineStore.Contracts.Product;
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
        private readonly IMapper _mapper;


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
            _mapper = mapper;
        }


        /// <inheritdoc/>
        public async Task AddProductToCartAsync(int productId, int quantity, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
                ?? throw new NullReferenceException();

            var existingCart = await _cartRepository.GetCartByUserAsync(user.Id, cancellation);

            var product = await _productsService.GetProductByIdAsync(productId, cancellation);

            if (existingCart == null)
            {
                existingCart = new Cart
                {
                    UserId = user.Id,
                    Created = _dataTimeProvider.Now,
                    StatusId = (int)CartStatusEnum.New,
                };

                AddProductToCart(existingCart, product, quantity);

                await _cartRepository.AddAsync(existingCart, cancellation);
            }
            else
            {

                AddProductToCart(existingCart, product, quantity);
                existingCart.Updated = _dataTimeProvider.Now;
                await _cartRepository.UpdateAsync(existingCart, cancellation);
            }

        }

        /// <summary>
        /// Проверяет если такой товар в корзину уже есть прибавляет количество, если нет доваляет к другим в корзину
        /// </summary>
        /// <param name="cart">Корзина</param>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="quantity">Количество продукта</param>
        private static void AddProductToCart(Cart cart, ShortProductDto shortProduct, int quantity)
        {
            var productInCart = cart.Products.FirstOrDefault(p => p.ProductId == shortProduct.Id);

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
                    Price = shortProduct.Price,
                    productName = shortProduct.Name,
                    ProductId = shortProduct.Id,
                });

            }
        }

        /// <inheritdoc/>
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

            return await GetCartItemsAsync(Cart, cancellation);
        }

        /// <inheritdoc/>
        public async Task<int?> GetCartItemCountAsync(CancellationToken cancellation)
        {
            var exictingCart = await GetCurrentUserCartAsync(cancellation);
            return exictingCart?.Products?.Sum(x => x.Quantity) ?? 0;
        }

        /// <summary>
        /// Получает корзину текущего пользователя
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        private async Task<Cart> GetCurrentUserCartAsync(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User) ??
                throw new NullReferenceException();

            return await _cartRepository.GetCartByUserAsync(user.Id, cancellation);
        }

        /// <summary>
        /// Создает CartDto и помещает туда продукты из корзины
        /// </summary>
        /// <param name="cart">Корзина</param>
        /// <param name="cancellation">Токен отмены операции</param>
        private async Task<CartDto> GetCartItemsAsync(Cart cart, CancellationToken cancellation)
        {
            var item = new List<CartItemDto>(cart.Products.Count);

            var totalAmount = 0m;

            foreach (var product in cart.Products)
            {
                item.Add(new CartItemDto
                {
                    Price = product.Price,
                    Quantity = product.Quantity,
                    ProductName = product.productName,
                    ProductId = product.ProductId,
                });
                totalAmount += product.Quantity * product.Price;
            }

            return new CartDto
            {
                Items = item,
                TotalAmount = Math.Round(totalAmount, 2)
            };
        }

        /// <inheritdoc/>
        public async Task RemoveItemAsync(int productId, CancellationToken cancellation)
        {
            var cart = await GetCurrentUserCartAsync(cancellation)
            ?? throw new InvalidOperationException("Не найдена корзина текущего пользователя");

            var productInCart = cart.Products.FirstOrDefault(x => x.ProductId == productId)
                ?? throw new InvalidOperationException("Не найден товар в корзине текущего пользователя для удаления");

            cart.Products.Remove(productInCart);

            await _cartRepository.UpdateAsync(cart, cancellation);
        }

        /// <inheritdoc/>
        public async Task RemoveAllItemAsync(CancellationToken cancellation)
        {
            var cart = await GetCurrentUserCartAsync(cancellation)
            ?? throw new InvalidOperationException("Не найдена корзина текущего пользователя");

            cart.Products.Clear();

            await _cartRepository.UpdateAsync(cart, cancellation);
        }

        /// <inheritdoc/>
        public async Task<CartItemDto> GetCartItemId(int id, CancellationToken cancellation)
        {
            var userId = await GetCurrentUserCartAsync(cancellation) 
                ?? throw new InvalidOperationException("Не найдена корзина текущего пользователя");

            return _mapper.Map<CartItemDto>(await _cartRepository.GetCartItemId(id, userId.Id, cancellation));
        }
    }
}
