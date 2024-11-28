using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Helpers;
using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.Contracts.Carts;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Carts.Services
{
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
        public async Task AddProductToCartAsync(int productId, CancellationToken cancellation)
        {
            await _cartService.AddProductToCartAsync(productId, cancellation);

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

        public async Task RemoveItemAsync(int productId, CancellationToken cancellation)
        {
            await _cartService.RemoveItemAsync(productId, cancellation);

            var user = await _userManager.GetUserAsync( _httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return;
            }

            await _cacheService.RemoveAsync(CartRedisKeyHelper.GetCartItemsCountKey(user.Id), cancellation);
        }
    }
}
