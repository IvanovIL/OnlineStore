using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Common;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Common
{
	public class EfRepositoryBase<T> : IRepository<T> where T : class
	{
		public readonly MutableOnlineStoreDbContext _mutableDbContext;
		public readonly ReadOnlyOnlineStoreDbContext _readOnlydbContext;

		public EfRepositoryBase(MutableOnlineStoreDbContext mutabledbContext,
			ReadOnlyOnlineStoreDbContext readOnlydbContext)
		{
			_mutableDbContext = mutabledbContext;
			_readOnlydbContext = readOnlydbContext;
		}

		/// <inheritdoc/>
		public async Task AddAsync(T entity)
		{
			  _mutableDbContext.AddAsync(entity);
			await _mutableDbContext.SaveChangesAsync();

		}

        /// <inheritdoc/>
        public Task AddAsync(T entity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

		/// <inheritdoc/>
		public async virtual Task<List<T>> GetAllAsync()
		{
			return await _readOnlydbContext.Set<T>().ToListAsync();
		}

        /// <inheritdoc/>
        public Task<List<T>> GetAllAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync(Product product, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async virtual Task<T> GetAsync(int id)
		{
			return await _readOnlydbContext.FindAsync<T>(id);
		}

		/// <inheritdoc/>
		public Task<Product> GetProductsAsync(GetProductsRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
