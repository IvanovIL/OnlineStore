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
		public Task<T> GetOrSetAsync<T>(string key, TimeSpan lifeTime, Func<Task<T>> func, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public Task RemoveAsync(string key, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}
	}
}
