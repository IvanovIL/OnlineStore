
using OnlineStore.AppServices.Products.Models;
using OnlineStore.Domain.Entities;

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
		/// Добавляет сущность
		/// </summary>
		void Add(int id);
		Task AddAsync(T entity);

		/// <summary>
		/// Получает все записи 
		/// </summary>
		Task <List<T>> GetAllAsync();
		Task<Product> GetProductsAsync(GetProductsRequest request);
	}
}
