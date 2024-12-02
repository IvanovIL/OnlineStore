using OnlineStore.Contracts.Carts;

namespace OnlineStore.AppServices.Carts.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе с корзиной
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Добавляет товар в корзину текущего пользователя
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task AddProductToCartAsync(int productId,int quantity,CancellationToken cancellation);

        /// <summary>
        /// Возвращает количество товара у текущего пользователя
        /// </summary>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task<int?> GetCartItemCountAsync(CancellationToken cancellation);

        /// <summary>
        /// Получает содержимое корзины товаров
        /// </summary>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task<CartDto> GetCartAsync(CancellationToken cancellation);

        /// <summary>
        /// Удаляет товар из корзины.
        /// </summary>
        /// <param name="productId">Идентификатор товара.</param>
        /// <param name="cancellation">Токен отмены операции.</param>
        Task RemoveItemAsync(int productId, CancellationToken cancellation);
    }
}
