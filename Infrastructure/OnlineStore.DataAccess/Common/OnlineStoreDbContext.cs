using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Common
{
	/// <summary>
	/// Контекст для работы с БД
	/// </summary>
	public class OnlineStoreDbContext : DbContext
	{
        public OnlineStoreDbContext()
        {
            
        }

        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options) 
			: base(options)
        {
           
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ProductAttribute>(entity =>
			{
				entity.ToTable("Attributes");
				entity.HasKey(e => e.Id);
			}
			);
		}
		DbSet<ProductAttribute> attributes { get;set; }

	}
}
