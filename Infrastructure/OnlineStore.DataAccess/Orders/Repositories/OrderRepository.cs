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

        public Task<List<Order>> GetOrdersAsync(int userId)
        {
            return _readOnlydbContext.Set<Order>()
                .Where(c => c.UserId == userId)
                .Where(c => c.OrderStatusId != 6)
                 .Include(c => c.orderItems.Where(cr => !cr.IsDeleted))
                .ToListAsync();
        }

        public Task<Order> GetOrderAsync(int OrderId)
        {
            return _readOnlydbContext.Set<Order>()
                .Where(c => c.Id == OrderId)
                .Where(c => c.OrderStatusId != (int)OrderStatusEnum.Canceled)
                 .Include(c => c.orderItems.Where(cr => !cr.IsDeleted))
                .FirstOrDefaultAsync();
        }

        public Task<List<OrderItem>> GetOrderItemAsync(int orderId)
        {
            return _mutableDbContext.Set<OrderItem>()
                .Where(c => c.OrderId == orderId)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public Task<OrderItem> DeleteProductOrderAsync(int orderId, int productId)
        {
            return _mutableDbContext.Set<OrderItem>()
                .Where(c => !c.IsDeleted)
                .Where(c => c.OrderId == orderId)
                .Where(c => c.ProductId == productId)
                 .FirstOrDefaultAsync();
        }

        public async Task updateOrderItem(OrderItem orderItem, CancellationToken cancellation)
        {
             _mutableDbContext.Update(orderItem);
            await _mutableDbContext.SaveChangesAsync(cancellation);

        }
    }
}
