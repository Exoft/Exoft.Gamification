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
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Exoft", "yelyzaveta.brednieva.pz.2016@lpnu.ua"));
            emailMessage.To.Add(new MailboxAddress("", "ostap2308@gmail.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);

                await client.AuthenticateAsync("yelyzaveta.brednieva.pz.2016@lpnu.ua", "03.02.1999");

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
