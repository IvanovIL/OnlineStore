using MimeKit;
using OnlineStore.Contracts.Notifications;
using MailKit.Net.Smtp;

namespace OnlineStore.AppServices.Common.NotificationServices
{
    /// <summary>
    /// Сервис отправки уведомлений на Email
    /// </summary>
    public sealed class EmailNotificationService : INotificationService
    {

        public async Task SendNotificationAsync(NotificationDto notification, CancellationToken cancellation)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("OnlineStore", "01ilya37@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", notification.Email));
            emailMessage.Subject = notification.Theme;
            emailMessage.Body = new TextPart("Добавлен новый продукт!234253")
            {
                Text = notification.Text

            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 587, MailKit.Security.SecureSocketOptions.StartTls, cancellation);
                await client.AuthenticateAsync("01ilya37@mail.ru", "Qiia4Xkk8nejaVmCLxpc", cancellation);
                await client.SendAsync(emailMessage, cancellation);
                
                
                 await client.DisconnectAsync(true);
            }
           
        }
    }
}
