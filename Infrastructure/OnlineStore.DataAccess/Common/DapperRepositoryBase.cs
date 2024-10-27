using Dapper;
using OnlineStore.AppServices.Common;



namespace OnlineStore.DataAccess.Common
{
	public class DapperRepositoryBase<T> : IRepository<T> where T : class
	{
		private readonly DbContext _context;

		protected DapperRepositoryBase(DbContext context)
		{
			_context = context;
		}

		public void Add(int id)
		{
			throw new NotImplementedException();
		}

		public async Task AddAsync(T entity)
		{
			await _context.connection.ExecuteAsync("");
		}

		public T Get(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetAsync(int id)
		{
			var query = $"SELECT * FROM {typeof(T).Name}s where Id = @Id";
			var result = await _context.connection.QueryFirstAsync<T>(query, new { Id = id });
			return result;
		}

	}
}
