using MimeKit;
using OnlineStore.Contracts.Notifications;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace OnlineStore.AppServices.Common.NotificationServices
{
    /// <summary>
    /// Сервис отправки уведомлений на Email
    /// </summary>
    public sealed class EmailNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;


        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendNotificationAsync(NotificationDto notification, CancellationToken cancellation)
        {
            using var emailMessage = new MimeMessage();

            var Email = _configuration["Email:Login"];
            var Password = _configuration["Email:Password"];

            emailMessage.From.Add(new MailboxAddress("OnlineStore", Email));
            emailMessage.To.Add(new MailboxAddress("", notification.Email));
            emailMessage.Subject = notification.Theme;
            emailMessage.Body = new TextPart()
            {
                Text = notification.Text
            };

       
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellation);
                await client.AuthenticateAsync(Email, Password, cancellation);
                await client.SendAsync(emailMessage, cancellation);

                await client.DisconnectAsync(true);
            }
        }
    }
}
