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
            
            TimeToExpireSecretString = TimeSpan.Parse(section.GetValue<string>("TimeToExpireSecretString"));
        }

        public TimeSpan TimeToExpireSecretString { get; }
    }
}
