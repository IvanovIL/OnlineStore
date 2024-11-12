using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Common
{
	/// <summary>
	/// Контекст для работы с БД
	/// </summary>
	public class MutableOnlineStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
	{
		public MutableOnlineStoreDbContext() : base()
		{

		}

		public MutableOnlineStoreDbContext(DbContextOptions<MutableOnlineStoreDbContext> options)
			: base(options)
		{

		}

		/// <inheritdoc/>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MutableOnlineStoreDbContext).Assembly);
		}

		/// <inheritdoc/>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

		}
	}
}
