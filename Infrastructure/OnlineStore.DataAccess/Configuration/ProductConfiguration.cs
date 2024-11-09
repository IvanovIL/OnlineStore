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

			//builder.HasData(

			//new Product
			//{
			//	Id = 1,
			//	Name = "Достоевский Р.Ф.<Преступление и наказиние>",
			//	Description = "Книга в твердом переплёте, 350 страниц.",
			//	Price = 250,
			//	CategoryId = 1,
			//	ImageUrl = null,
			//	stockQuantity = 30
			//},
			//new Product
			//{
			//	Id = 1,
			//	Name = "Пальто <Misteks design>",
			//	Description = "Пальто выполнено из драпа. Модель прямого кроя.Цвет черный.",
			//	Price = 700,
			//	CategoryId = 2,
			//	ImageUrl = null,
			//	stockQuantity = 12
			//},
			//new Product
			//{
			//	Id = 1,
			//	Name = "Телевизор Samsung Crystal UHD",
			//	Description = "Разрешение составляет 3840x2160 пикс с диагональю 55 и " +
			//		"обеспечивает высококачественную передачу изображения.",
			//	Price = 1000,
			//	CategoryId = 3,
			//	ImageUrl = null,
			//	stockQuantity = 6
			//}
			//	);
		}
	}
}
