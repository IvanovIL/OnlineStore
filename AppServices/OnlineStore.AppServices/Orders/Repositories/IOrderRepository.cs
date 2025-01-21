using OnlineStore.AppServices.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Orders.Repositories
{
	public interface IOrderRepository : IRepository<Order>
	{

        Task<List<Order>> GetOrdersAsync(int userId);

        Task<Order> GetOrderAsync(int OrderId);

        Task<List<OrderItem>> GetOrderItemAsync(int orderId);

        Task<OrderItem> DeleteProductOrderAsync(int orderId, int productId);

        Task updateOrderItem(OrderItem orderItem, CancellationToken cancellation);

    }
}
