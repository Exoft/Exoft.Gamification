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
            EmailForSendMessage = secretSection.GetValue<string>("EmailForSendMessage");
            Password = secretSection.GetValue<string>("Password");
        }


        public byte[] Secret { get; }

        public string EmailForSendMessage { get; }

        public string Password { get; }
}
}
