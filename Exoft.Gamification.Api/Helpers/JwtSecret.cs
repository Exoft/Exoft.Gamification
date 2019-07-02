using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace Exoft.Gamification.Api.Helpers
{
    public class JwtSecret : IJwtSecret
    {
        public JwtSecret(IConfiguration configuration)
        {
            var secretSection = configuration.GetSection("Secrets");
            Secret = Encoding.ASCII.GetBytes(secretSection.GetValue<string>("TokenSecretString"));
            TimeToExpireToken = TimeSpan.FromSeconds(secretSection.GetValue<int>("SecondsToExpireToken"));
            TimeToExpireRefreshToken = TimeSpan.FromSeconds(secretSection.GetValue<int>("SecondsToExpireRefreshToken"));
        }


        public byte[] Secret { get; }

        public TimeSpan TimeToExpireToken { get; }

        public TimeSpan TimeToExpireRefreshToken { get; }
    }
}
