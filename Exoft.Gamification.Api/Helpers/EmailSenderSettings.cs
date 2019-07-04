using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;
using System;

namespace Exoft.Gamification.Api.Helpers
{
    public class EmailSenderSettings : IEmailSenderSettings
    {
        public EmailSenderSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("EmailSenderSettings");

            Email = section.GetValue<string>("Mail");
            Password = section.GetValue<string>("Password");
            SmtpClient = section.GetValue<string>("SmtpClient");
            Port = section.GetValue<int>("Port");
            EnableSsl = section.GetValue<bool>("EnableSsl");
            DisplayName = section.GetValue<string>("DisplayName");
            ResetPasswordPage = section.GetValue<string>("ResetPasswordPage");
            TimeToExpireSecretString = TimeSpan.Parse(section.GetValue<string>("TimeToExpireSecretString"));
        }

        public string Email { get; }

        public string Password { get; }

        public string SmtpClient { get; }

        public int Port { get; }

        public bool EnableSsl { get; }

        public string DisplayName { get; set; }

        public string ResetPasswordPage { get; }

        public TimeSpan TimeToExpireSecretString { get; }
    }
}
