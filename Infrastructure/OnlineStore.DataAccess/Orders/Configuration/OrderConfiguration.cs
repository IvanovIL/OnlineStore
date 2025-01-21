using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;


namespace OnlineStore.DataAccess.Orders.Configuration
{
    public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.userName)
               .HasMaxLength(1000)
               .IsRequired(true);

            builder.Property(t => t.numberPhoneUser)
              .HasMaxLength(100)
              .IsRequired(true);

            builder.Property(t => t.addressUser)
             .HasMaxLength(1000)
             .IsRequired(true);

            builder.Property(t => t.OrderDate)
             .IsRequired(true);

            builder.Property(t => t.TotalAmount)
               .HasPrecision(30, 4)
               .IsRequired(true);

            builder.Property(t => t.UserId)
                .IsRequired(true);

            builder.HasOne(x => x.OrderStatus)
                .WithMany()
                .HasForeignKey(x => x.OrderStatusId)
                .IsRequired(true);

        


        }
    }
}
