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
			builder.HasKey(e => e.Id);

			builder.Property(t => t.Id).
				HasColumnName("Id");

			builder.Property(t => t.Name).
				HasColumnName("Name").IsRequired(true);

			builder.Property(t => t.Price).
				HasColumnName("Price").IsRequired(true);

			
		}
	}
}
