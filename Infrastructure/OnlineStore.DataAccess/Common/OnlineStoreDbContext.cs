using Microsoft.Data.SqlClient;

namespace OnlineStore.DataAccess.Common
{
	/// <summary>
	/// Контекст для работы с БД
	/// </summary>
	public class OnlineStoreDbContext : IDisposable
	{
		private readonly SqlConnection _connection;

		public OnlineStoreDbContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

		public SqlConnection connection => _connection;
		
        public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
