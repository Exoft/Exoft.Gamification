using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;
using System;

namespace Exoft.Gamification.Api.Helpers
{
    public class ResetPasswordSettings : IResetPasswordSettings
    {
        public ResetPasswordSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("ResetPasswordSettings");
            
            ResetPasswordPage = section.GetValue<string>("ResetPasswordPage");
            TimeToExpireSecretString = TimeSpan.Parse(section.GetValue<string>("TimeToExpireSecretString"));
        }

        public string ResetPasswordPage { get; }

        public TimeSpan TimeToExpireSecretString { get; }
    }
}
