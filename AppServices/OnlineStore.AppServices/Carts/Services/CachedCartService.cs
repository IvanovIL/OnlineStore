using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Helpers;
using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.Contracts.Carts;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Carts.Services
{
        /// <summary>
        /// Кэширующий сервис по работе с корзиной
        /// </summary>
    public sealed class CachedCartService : ICartService
    {
        private readonly ICartService _cartService;
        private readonly ICacheService _cacheService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CachedCartService(ICartService cartService,
            ICacheService cacheService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _cacheService = cacheService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }

        /// <inheritdoc/>
        public async Task AddProductToCartAsync(int productId, int quantity, CancellationToken cancellation)
        {
            await _cartService.AddProductToCartAsync(productId, quantity, cancellation);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return;
            }

            await _cacheService.RemoveAsync(CartRedisKeyHelper.GetCartItemsCountKey(user.Id), cancellation);
        }

        /// <inheritdoc/>
        public Task<CartDto> GetCartAsync(CancellationToken cancellation)
        {
            return _cartService.GetCartAsync(cancellation);
        }


        /// <inheritdoc/>
        public async Task<int?> GetCartItemCountAsync(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return null;
            }

            return await _cacheService.GetOrSetAsync(
                key: CartRedisKeyHelper.GetCartItemsCountKey(user.Id),
                lifeTime: TimeSpan.FromMinutes(60),
                func: async () => (await _cartService.GetCartItemCountAsync(cancellation)),
                cancellation: cancellation
                );

        }

        public Task<CartItemDto> GetCartItemId(int id, CancellationToken cancellation)
        {
            return _cartService.GetCartItemId(id, cancellation);
        }

        public Task<CartDto> GetProductCart(int id, int userId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task RemoveAllItemAsync(CancellationToken cancellation)
        {
            await _cartService.RemoveAllItemAsync(cancellation);
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return;
            }

            await _cacheService.RemoveAsync(CartRedisKeyHelper.GetCartItemsCountKey(user.Id), cancellation);
        }

        /// <inheritdoc/>
        public async Task RemoveItemAsync(int productId, CancellationToken cancellation)
        {
            await _cartService.RemoveItemAsync(productId, cancellation);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return;
            }

            await _cacheService.RemoveAsync(CartRedisKeyHelper.GetCartItemsCountKey(user.Id), cancellation);
        }
    }
}
