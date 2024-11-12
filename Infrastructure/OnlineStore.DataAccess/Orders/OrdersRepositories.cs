using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Orders
{
	public sealed class OrdersRepositories : EfRepositoryBase<Order>, IOrderRepository
	{
		/// <summary>
		/// Репозитории по работе с заказами
		/// </summary>
		public OrdersRepositories(MutableOnlineStoreDbContext mutabledbContext, 
			ReadOnlyOnlineStoreDbContext readOnlydbContext) :
			base(mutabledbContext, readOnlydbContext)
		{

		}
	}
}
