using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.Contracts.Enums;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Orders.Repositories
{
    public sealed class OrderRepository : EfRepositoryBase<Order>, IOrderRepository
    {
        /// <summary>
        /// Репозитории по работе с заказами
        /// </summary>
        public OrderRepository(MutableOnlineStoreDbContext mutabledbContext,
            ReadOnlyOnlineStoreDbContext readOnlydbContext) :
            base(mutabledbContext, readOnlydbContext)
        {

        }

        public Task<List<Order>> GetOrderByUserAsync(int userId, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<Order>()
                 .Where(c => c.UserId == userId)
                 .Include(c => c.orderItems)
                 .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _readOnlydbContext.Set<Order>()
                .Include(x => x.UserId)
                .ToListAsync();
        }

    }
}
