using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSenderSettings _emailSenderSettings;

        public EmailService(IEmailSenderSettings emailSenderSettings)
        {
            _emailSenderSettings = emailSenderSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Exoft", _emailSenderSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            emailMessage.Body = builder.ToMessageBody();
                

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _emailSenderSettings.SmtpClient,
                    _emailSenderSettings.Port,
                    _emailSenderSettings.EnableSsl);

                await client.AuthenticateAsync(_emailSenderSettings.Email, _emailSenderSettings.Password);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
