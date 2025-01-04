using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Contracts.Enums;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Extensions;

namespace OnlineStore.DataAccess.Orders.Configuration
{
    public sealed class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatus");

            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(256);

            builder.HasData(
                Enum.GetValues(typeof(OrderStatusEnum))
                .Cast<OrderStatusEnum>()
                .Select(e => new CartStatus()
                {
                    Id = (int)e,
                    Name = e.GetEnumDescription()

                }));


        }
    }
}
