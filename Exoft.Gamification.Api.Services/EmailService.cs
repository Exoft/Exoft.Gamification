using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System.Collections.Generic;
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

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = GetSmtpClient();

            var mailMessage = CreateMailMessage(subject, message);
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailsAsync(ICollection<string> emails, string subject, string message)
        {
            var client = GetSmtpClient();

            var mailMessage = CreateMailMessage(subject, message);
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }

            await client.SendMailAsync(mailMessage);
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient(_emailSenderSettings.SmtpClient, _emailSenderSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSenderSettings.Email, _emailSenderSettings.Password),
                EnableSsl = _emailSenderSettings.EnableSsl
            };

            return client;
        }

        private MailMessage CreateMailMessage(string subject, string message)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSenderSettings.Email, _emailSenderSettings.DisplayName),
                IsBodyHtml = true,
                Body = message,
                Subject = subject
            };

            return mailMessage;
        }
    }
}
