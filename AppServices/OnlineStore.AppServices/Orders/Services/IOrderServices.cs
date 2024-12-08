

using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Order;
using OnlineStore.Contracts.Product;

namespace OnlineStore.AppServices.Orders.Services
{
    public interface IOrderServices
    {
        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <param name="request">Запрос на получение списка заказов</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ProductsListDto> GetOrderAsync(PagedRequest request, CancellationToken cancellation);

        /// <summary>
        /// Добавляет заказ
        /// </summary>
        /// <param name="orderDto">Транспортная модель заказа</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddOrderAsync(OrderDto orderDto, CancellationToken cancellation);


        /// <summary>
        /// Удаляет заказ
        /// </summary>
        /// <param name="productId">Идентификатор заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        /// <returns></returns>
        Task DeleteOrderAsync(int orderId, CancellationToken cancellation);
    }
}
