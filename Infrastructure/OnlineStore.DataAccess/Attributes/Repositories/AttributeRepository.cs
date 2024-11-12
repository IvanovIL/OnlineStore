using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Attributes.Repositories
{
	/// <summary>
	/// Репозиторий по работе с атрибутами
	/// </summary>
	public sealed class AttributeRepository : EfRepositoryBase<ProductAttribute>, IAttributeRepository
	{
		public AttributeRepository(MutableOnlineStoreDbContext dbContext, 
			ReadOnlyOnlineStoreDbContext readOnlydbContext) 
			: base(dbContext, readOnlydbContext)
		{
		}
	}
}
