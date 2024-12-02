

namespace OnlineStore.AppServices.Common
{
	/// <summary>
	/// Интерфейс общего репозитория
	/// </summary>
	public interface IRepository<T> where T : class
	{

        /// <summary>
        /// Получает сущность по идентификотору
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<T> GetAsync(int id);


        /// <summary>
        /// Добавляет сущность
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddAsync(T entity, CancellationToken cancellation);

        /// <summary>
        /// Получает все записи 
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<List<T>> GetAllAsync(CancellationToken cancellation);

        /// <summary>
        /// Обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task UpdateAsync(T entity, CancellationToken cancellation);

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="id">Токен отмены операции</param>
        Task DeleteAsync( T entity, CancellationToken cancellation);

    }
}
