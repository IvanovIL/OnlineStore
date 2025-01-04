using OnlineStore.AppServices.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Orders.Repositories
{
	public interface IOrderRepository : IRepository<Order>
	{

        Task<List<Order>> GetOrdersAsync(int userId);

        Task<List<Order>> GetOrderByUserAsync(int userId, CancellationToken cancellation);

    }
}
