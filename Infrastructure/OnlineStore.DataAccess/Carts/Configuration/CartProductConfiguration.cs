using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;


namespace OnlineStore.DataAccess.Carts.Configuration
{
    internal class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {

            builder.ToTable("cartProduct");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
              .IsRequired(true)
            .ValueGeneratedOnAdd();

     

            builder.HasOne(cp => cp.Cart)
                .WithMany(c => c.Products)
                .HasForeignKey(cp => cp.CartId)
                .IsRequired(true);


            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .IsRequired(true);
        }
    }
}
