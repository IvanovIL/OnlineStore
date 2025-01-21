using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Contracts.Enum;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Extensions;

namespace OnlineStore.DataAccess.Notification.Configuration
{
	public sealed class NotificationChannelConfiguration : IEntityTypeConfiguration<NotificationChannel>
	{
		public void Configure(EntityTypeBuilder<NotificationChannel> builder)
		{
			builder.ToTable("NotificationChannel");

			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(256);

			builder.HasData(Enum.GetValues(typeof(NotificationChannelEnum))
				.Cast<NotificationChannelEnum>()
				.Select(e => new NotificationChannel
				{
					Id = (int)e,
					Name = e.GetEnumDescription()
				}));
		}
	}
}
