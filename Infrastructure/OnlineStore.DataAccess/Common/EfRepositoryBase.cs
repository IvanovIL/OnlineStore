using OnlineStore.AppServices.Common;

namespace OnlineStore.DataAccess.Common
{
	public class EfRepositoryBase<T> : IRepository<T> where T : class
	{
		private readonly OnlineStoreDbContext _dbContext;

		public EfRepositoryBase(OnlineStoreDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void Add(int id)
		{
			throw new NotImplementedException();
		}

		public async Task AddAsync(T entity)
		{
			 await _dbContext.AddAsync(entity);
		}

		public T Get(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetAsync(int id)
		{
			return await _dbContext.FindAsync<T>(id);
		}

	}
}
