using Microsoft.Extensions.Configuration;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.DataAccess.Common;

namespace OnlineStore.DataAccess.Attributes.Repositories
{
	/// <summary>
	/// Репозиторий по работе с атрибутами
	/// </summary>
	public sealed class AttributeRepository : DapperRepositoryBase<Attribute>, IAttributeRepository
	{
		public AttributeRepository(DbContext context) : base(context)
		{
		}
	}
}
