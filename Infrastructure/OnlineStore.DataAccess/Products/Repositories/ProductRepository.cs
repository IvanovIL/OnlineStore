using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;


namespace OnlineStore.DataAccess.Products.Repositories
{
	/// <summary>
	/// Репозитории по работе с товарами
	/// </summary>
	public sealed class ProductRepository : EfRepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(MutableOnlineStoreDbContext dbContext,
			ReadOnlyOnlineStoreDbContext readOnlydbContext) 
			: base(dbContext, readOnlydbContext)
		{

		} 


		/// <inheritdoc/>
		public async override Task<List<Product>> GetAllAsync()
		{
			return await _readOnlydbContext.Set<Product>()
				.Include(x => x.Category)
				.ToListAsync();
		}

		/// <inheritdoc/>
		public Task<List<Product>> GetProductsAsync(GetProductsRequest request)
		{
			var query = _readOnlydbContext.Set<Product>().AsQueryable();

			if (request.IncludeCategory)
			{
				query = query.Include(x => x.Category);
			}

			//if(request.IncludeImages)
			//{
			//	query = query.Include(x => x.Images);
			//}

			return query.ToListAsync();
		}

		/// <inheritdoc/>
		//public async override Task<Product> GetAsync(int id)
		//{

		//	var entity =  await _readOnlydbContext.FindAsync<Product>(id);

		//	await _readOnlydbContext.Entry(entity)
		//		.Reference(x => x.Category)
		//		.LoadAsync();

		//	return entity;
		//}
	}
}
