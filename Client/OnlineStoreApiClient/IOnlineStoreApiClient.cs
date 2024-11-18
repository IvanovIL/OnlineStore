

namespace OnlineStoreApiClients
{
    /// <summary>
    /// Интерфейс Апи-клиента
    /// </summary>
    public interface IOnlineStoreApiClient
    {
        /// <summary>
        /// Добавляет новый продукт
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        /// <returns></returns>
        public Task AddProductAsync(object body,CancellationToken cancellation);
    }
}
