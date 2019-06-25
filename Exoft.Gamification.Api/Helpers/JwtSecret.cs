using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Exoft.Gamification.Api.Helpers
{
    public class JwtSecret : IJwtSecret
    {
        public JwtSecret(IConfiguration configuration)
        {
            var secretSection = configuration.GetSection("Secrets");
            Secret = Encoding.ASCII.GetBytes(secretSection.GetValue<string>("TokenSecretString"));
            SecondsToExpireToken = secretSection.GetValue<int>("SecondsToExpireToken");
            SecondsToExpireRefreshToken = secretSection.GetValue<int>("SecondsToExpireRefreshToken");
        }


        public byte[] Secret { get; }

        public int SecondsToExpireToken { get; }

        public int SecondsToExpireRefreshToken { get; }
    }
}
