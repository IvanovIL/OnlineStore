


namespace OnlineStore.AppServices.Common.Redis
{
	public sealed class RedisCache : IRedisCache
	{
		public Task<T> GetAsync<T>(string key)
		{
			throw new NotImplementedException();
		}

		public Task SetAsync<T>(string key, T value, TimeSpan lifeTime)
		{
			throw new NotImplementedException();
		}
	}
}
