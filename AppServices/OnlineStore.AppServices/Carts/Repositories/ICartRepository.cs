using OnlineStore.AppServices.Common;
using OnlineStore.Domain.Entities;


namespace OnlineStore.AppServices.Carts.Repositories
{
    /// <summary>
    /// Интерфейс репозитория корзины
    /// </summary>
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Получает идентификатор пользователя корзины
        /// </summary>
        /// <param name="userId">Иднетификатор пользователя</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<Cart> GetCartByUserAsync(int userId,CancellationToken cancellation);

        /// <summary>
        /// Получает продукт из корзины по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<CartProduct> GetCartItemId(int id, int userId, CancellationToken cancellation);
    }
}
