using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Categories.Configuration
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(e => e.Id);

            builder.Property(t => t.Id).
                HasColumnName("Id");

            builder.Property(t => t.Name).
                HasColumnName("Name").IsRequired(true);


            builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Книги"
            },
            new Category
            {
                Id = 2,
                Name = "Одежда",
            },
            new Category
            {
                Id = 3,
                Name = "Электроника",
            });


        }
    }
}
