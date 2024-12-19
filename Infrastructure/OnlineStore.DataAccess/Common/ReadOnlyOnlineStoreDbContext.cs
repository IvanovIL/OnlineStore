using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Common
{
	public  class ReadOnlyOnlineStoreDbContext  : IdentityDbContext<ApplicationUser, ApplicationRole, int>
	{
		public ReadOnlyOnlineStoreDbContext() : base()
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
