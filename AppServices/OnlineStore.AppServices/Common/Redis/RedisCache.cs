using StackExchange.Redis;

namespace OnlineStore.AppServices.Common.Redis
{
	public sealed class RedisCache : IRedisCache
	{
		private readonly IDatabase _redisDb;

		public RedisCache( IDatabase redisDb)
		{
			_redisDb = redisDb;
		}

		/// <inheritdoc/>
		public async Task<string> GetAsync(string key)
		{
			var entity = await _redisDb.StringGetAsync(key);
			return entity;
		}
		
		/// <inheritdoc/>
		public async Task SetStringAsync<T>(string key, RedisValue value)
		{
			await _redisDb.SetAddAsync(key, value);
			await _redisDb.StringSetAsync(key, value);
		}

		Task<T> IRedisCache.GetAsync<T>(string key, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}

		Task IRedisCache.RemoveAsync(string key, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}

		Task IRedisCache.SetAsync<T>(string key, T value, TimeSpan lifeTime, CancellationToken cancellation)
		{
			throw new NotImplementedException();
		}
	}
}