using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Orders
{
	public sealed class OrdersRepositories : DapperRepositoryBase<Order>, IOrderRepository
	{
		public OrdersRepositories(OnlineStoreDbContext context) : base(context)
		{

		}
	}
}
