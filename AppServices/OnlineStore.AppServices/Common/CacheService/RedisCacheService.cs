using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.AppServices.Common.Redis;

namespace OnlineStore.AppServices.Common.CacheService
{
	/// <summary>
	/// Сервис кэширования в редис
	/// </summary>
	public sealed class RedisCacheService : ICacheService
	{
		private readonly IRedisCache _redisCacheService;
		/// <inheritdoc/>
		public RedisCacheService(IRedisCache redisCacheService)
        {
			_redisCacheService = redisCacheService;
		}

		/// <inheritdoc/>
		public async Task<T> GetOrSetAsync<T>(string key, TimeSpan lifeTime, Func<Task<T>> func, CancellationToken cancellation)
		{
			var cacheItem =await  _redisCacheService.GetAsync<T>(key, cancellation);

			if (cacheItem != null)
			{
				return cacheItem;
			}

			var Item = await func();

			if (Item != null)
			{
				await _redisCacheService.SetAsync(key, Item, lifeTime, cancellation);
			}
			return Item;
		}

		/// <inheritdoc/>
		public async Task RemoveAsync(string key, CancellationToken cancellation)
		{
			await _redisCacheService.RemoveAsync(key,cancellation);
		}
	}
}
