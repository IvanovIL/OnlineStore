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
		public ProductRepository(MutableOnlineStoreDbContext mutabledbContext,
			ReadOnlyOnlineStoreDbContext readOnlydbContext) 
			: base(mutabledbContext, readOnlydbContext)
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
		public Task<List<Product>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellation)
		{
            var query = _readOnlydbContext
               .Set<Product>()
               .AsQueryable()
               .Where(p => !p.IsDeleted);

            if (request.IncludeCategory)
            {
                query = query
                    .Include(x => x.Category);
            }

            if (request.IncludeImages)
            {
                query = query
                    .Include(x => x.Images);
            }

            query = query
                .OrderBy(x => x.Id)
                .Skip(request.Skip);

            if (request.Take != default)
            {
                query = query.Take(request.Take);
            }

            return query.ToListAsync(cancellation);
        }

		/// <inheritdoc/>
		public override Task<Product> GetAsync(int id)
		{
			return _readOnlydbContext.Set<Product>()
				.Where(x => x.Id == id)
				.Where(x => !x.IsDeleted)
				.Include(p => p.Images)
				.FirstOrDefaultAsync();
		}

        public Task<int> GetProductsTotalCountAsync(CancellationToken cancellation)
        {
            return _readOnlydbContext
                .Set<Product>()
                .Where(p => !p.IsDeleted)
                .CountAsync(cancellation);
        }
    }
}
