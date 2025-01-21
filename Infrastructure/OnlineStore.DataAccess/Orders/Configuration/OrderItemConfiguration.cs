using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Orders.Configuration
{
    public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Quantity);

            builder.Property(t => t.UnitPrice)
              .HasPrecision(30, 4)
              .IsRequired(true);

            builder.HasOne(cp => cp.Order)
                .WithMany(c => c.orderItems)
                .HasForeignKey(cp => cp.OrderId)
                .IsRequired(true);

            builder.HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId)
                .IsRequired(true);

        }
    }
}
