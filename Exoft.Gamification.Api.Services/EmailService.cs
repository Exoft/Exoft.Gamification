using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IJwtSecret _jwtSecret;

        public EmailService(IJwtSecret jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string emailAdmin = _jwtSecret.EmailForSendMessage;
            string passwordAdmin = _jwtSecret.Password;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Exoft", emailAdmin));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);

                await client.AuthenticateAsync(emailAdmin, passwordAdmin);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
