using Exoft.Gamification.Api.Services.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string emailAdmin = "yelyzaveta.brednieva.pz.2016@lpnu.ua";
            string passwordAdmin = "03.02.1999";

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
