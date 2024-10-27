
namespace OnlineStore.AppServices.Common
{
	/// <summary>
	/// Интерфейс общего репозитория
	/// </summary>
	public interface IRepository<T> where T : class
	{
		T Get(int id);
		/// <summary>
		/// Получает сущность по идентификотору
		/// </summary>

		Task<T> GetAsync(int id);
		/// <summary>
		/// 
		/// </summary>
		void Add(int id);
		Task AddAsync(T entity);
	}
}
