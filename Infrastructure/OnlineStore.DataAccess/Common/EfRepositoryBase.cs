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

		public EfRepositoryBase(MutableOnlineStoreDbContext dbContext,
			ReadOnlyOnlineStoreDbContext readOnlydbContext)
		{
			_mutableDbContext = dbContext;
			_readOnlydbContext = readOnlydbContext;
		}

		/// <inheritdoc/>
		public void Add(int id)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public async Task AddAsync(T entity)
		{
			 await _mutableDbContext.AddAsync(entity);
		}

		/// <inheritdoc/>
		public T Get(int id)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public async virtual Task<List<T>> GetAllAsync()
		{
			return await _readOnlydbContext.Set<T>().ToListAsync();
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
