using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Contracts.Enum;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Extensions;

namespace OnlineStore.DataAccess.Configuration
{
	public sealed class ProductAttributeTypeConfiguration : IEntityTypeConfiguration<ProductAttributeType>
	{
		public void Configure(EntityTypeBuilder<ProductAttributeType> builder)
		{
			builder.ToTable("ProductAttributeType");

			builder.HasKey(e => e.Id);

			builder.Property(t => t.Name).IsRequired(true);


			builder.HasData(Enum.GetValues(typeof(ProductAttributeEnum))
				.Cast<ProductAttributeEnum>()
				.Select(e => new ProductAttributeType
				{
					Id = (int)e,
					Name = e.GetEnumDescription()
				}));
		}
	}
}
