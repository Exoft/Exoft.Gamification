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
            ExpireToken = secretSection.GetValue<string>("HoursToExpireToken");
            ExpireRefreshToken = secretSection.GetValue<string>("HoursToExpireRefreshToken");
        }


        public byte[] Secret { get; }

        public string ExpireToken { get; }

        public string ExpireRefreshToken { get; }
    }
}
