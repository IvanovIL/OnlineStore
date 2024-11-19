using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Configuration
{
	public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{

			builder.ToTable("Product");
			builder.HasKey(t => t.Id);

			builder.Property(t => t.Name)
				.HasMaxLength(1000)
				.IsRequired(true);

			builder.Property(t => t.Description)
				.IsRequired(true);

			builder.Property(t => t.Price)
				.HasPrecision(14, 4)
				.IsRequired(true);

			builder.HasOne(t => t.Category)
				.WithMany()
				.HasForeignKey(t => t.CategoryId)
				.IsRequired(false);

			builder.Property(t => t.ImageUrl)
				.IsRequired(false);

			builder.Property(t => t.stockQuantity)
				.IsRequired(true);

			builder.Property(t => t.createdAt)
				.IsRequired(true);


            builder.HasMany(t => t.Images)
				.WithOne(t => t.Product)
				.HasForeignKey(t => t.ProductId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Cascade);


		}
	}
}
