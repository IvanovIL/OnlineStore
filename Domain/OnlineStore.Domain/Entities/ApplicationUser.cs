using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Domain.Entities
{
	public sealed class ApplicationUser : IdentityUser<int>
	{
		/// <summary>
		/// Иднетификатор чата с пользователем в телеграм
		/// </summary>
		//public long? TelegramChatId { get; set; }

		/// <summary>
		/// Каналы уведомления пользователя
		/// </summary>
		public ICollection<NotificationChannel> NotificationChannels { get; set; } = [];
	}
}
