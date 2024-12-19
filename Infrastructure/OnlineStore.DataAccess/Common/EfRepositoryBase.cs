using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Common;
using OnlineStore.Contracts.Product;
using System.Diagnostics;


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
		public async Task AddAsync(T entity , CancellationToken cancellation)
		{
            await _mutableDbContext.AddAsync(entity, cancellation);
            await _mutableDbContext.SaveChangesAsync(cancellation);

        }

     

        public  Task DeleteAsync(T entity, CancellationToken cancellation)
        {
            _mutableDbContext.Update(entity);
            return _mutableDbContext.SaveChangesAsync(cancellation);
        }




        /// <inheritdoc/>
        public async virtual Task<List<T>> GetAllOrdersAsync()
		{
			return await _readOnlydbContext.Set<T>().ToListAsync();
		}

        /// <inheritdoc/>
        public virtual Task<List<T>> GetAllAsync(CancellationToken cancellation)
        {
            return _readOnlydbContext.Set<T>().ToListAsync(cancellation);
        }

		/// <inheritdoc/>
		public  virtual Task<T> GetAsync(int id)
		{
			return _readOnlydbContext.FindAsync<T>(id).AsTask();
		}

        /// <inheritdoc/>
        public  Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            
            _mutableDbContext.Update(entity);
            return _mutableDbContext.SaveChangesAsync(cancellation);
        }

        public async virtual Task<List<T>> FindAsync(string name)
        {
            return await _readOnlydbContext.Set<T>(name).ToListAsync();

        }


    }
}
