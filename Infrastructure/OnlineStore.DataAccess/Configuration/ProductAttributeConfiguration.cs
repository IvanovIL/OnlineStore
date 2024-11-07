using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Configuration
{
	public sealed class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
	{
		public void Configure(EntityTypeBuilder<ProductAttribute> builder)
		{
			builder.ToTable("Attributes");
			builder.HasKey(e => e.Id);

			builder.Property(t => t.Id).
				HasColumnName("Id");

			builder.Property(t => t.Name).
				HasColumnName("Name").IsRequired(true);

			builder.Property(t => t.Description).
				HasColumnName("Description");
		}
	}
}
