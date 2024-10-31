using Dapper;
using Dapper.Contrib.Extensions;
using OnlineStore.AppServices.Common;

namespace OnlineStore.DataAccess.Common
{
	public class DapperRepositoryBase<T> : IRepository<T> where T : class
	{
		private readonly OnlineStoreDbContext _context;

		protected DapperRepositoryBase(OnlineStoreDbContext context)
		{
			_context = context;
		}

		public void Add(int id)
		{
			throw new NotImplementedException();
		}

		public async Task AddAsync(T entity)
		{
			await _context.connection.ExecuteAsync($"INSERT INTO {typeof(T).Name}s (Name) VALUES (@Name)",entity);
		}

		public T Get(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetAsync(int id)
		{

			var result = await _context.connection.GetAsync<T>(id);
			return result;
		}

	}
}
