

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
        /// <param name="id">Идентификотор сущности</param>
        /// <returns></returns>
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
        /// Обновляет существующую сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task UpdateAsync(T entity, CancellationToken cancellation);

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task DeleteAsync( T entity, CancellationToken cancellation);

        /// <summary>
        /// Получает список сущностей по наименованию
        /// </summary>
        /// <param name="name">Наименование</param>
        Task<List<T>> FindAsync(string name);

        /// <summary>
        /// Получает список сущностей по идентификатору
        /// </summary>
        /// <param name="CategoryId">идентификатор</param>
        Task<List<T>> FindCategoryAsync(int CategoryId);


    }
}
