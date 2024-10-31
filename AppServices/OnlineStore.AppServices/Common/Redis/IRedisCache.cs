

namespace OnlineStore.AppServices.Common.Redis
{
	/// <summary>
	/// Интерфейс по работе с кэшем
	/// </summary>
	public interface  IRedisCache
	{
		/// <summary>
		/// Получает данные из Redis по ключу
		/// </summary>
		/// <param name="key"Ключ></param>
		Task<T> GetAsync<T>(string key);

		/// <summary>
		/// Записывает данные в Redis по указаному ключу
		/// </summary>
		/// <param name="key">Ключ</param>
		/// <param name="value">Данные</param>
		/// <param name="lifeTime">Время жизни значения в кэше</param>
		Task SetAsync<T>(string key, T value, TimeSpan lifeTime);
	}
}
