using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Carts.Configuration
{
    public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
                .IsRequired(true);

            builder.Property(t => t.Created)
                .IsRequired(true);

            builder.Property(t => t.Updated)
                .IsRequired(false);

            builder.Property(t => t.Closed)
                .IsRequired(false);


        }
    }
}
