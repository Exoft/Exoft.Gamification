using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System.Net;
using System.Net.Mail;
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

        public async Task SendEmailAsync(string email, string subject, string message, bool isMessageHtml = false)
        {
            SmtpClient client = new SmtpClient(_emailSenderSettings.SmtpClient, _emailSenderSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSenderSettings.Email, _emailSenderSettings.Password),
                EnableSsl = _emailSenderSettings.EnableSsl
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSenderSettings.Email, _emailSenderSettings.DisplayName);
            mailMessage.To.Add(email);
            mailMessage.IsBodyHtml = isMessageHtml;
            mailMessage.Body = message;
            mailMessage.Subject = subject;

            await client.SendMailAsync(mailMessage);
        }
    }
}
