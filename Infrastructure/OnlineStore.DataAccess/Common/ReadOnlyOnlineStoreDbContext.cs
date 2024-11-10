using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Common
{
	public  class ReadOnlyOnlineStoreDbContext : DbContext
	{
		public ReadOnlyOnlineStoreDbContext()
		{

		}

		public ReadOnlyOnlineStoreDbContext(DbContextOptions<ReadOnlyOnlineStoreDbContext> options)
			: base(options)
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		/// <inheritdoc/>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadOnlyOnlineStoreDbContext).Assembly);


		}
		/// <inheritdoc/>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

		}
	}
}
