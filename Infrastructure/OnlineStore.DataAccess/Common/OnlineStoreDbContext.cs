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
			modelBuilder.Entity<Category>(entity =>
			{
				entity.ToTable("Category");
				entity.HasKey(e => e.Id);
			}
			);
			modelBuilder.Entity<Product>(entity =>
			{
				entity.ToTable("Product");
				entity.HasKey(e => e.Id);
			}
			);
		}
		DbSet<ProductAttribute> attributes { get;set; }
		DbSet<Category> categories { get;set; }

		DbSet<Product>	products { get;set; }
	}
}
