using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;

namespace Exoft.Gamification.Api.Helpers
{
    public class AppCenterSettings : IAppCenterSettings
    {
        public AppCenterSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("AppCenterSettings");

            Token = section.GetValue<string>("Token");
            KeyNameToken = section.GetValue<string>("KeyNameToken");
            UrlForPushAndroid = section.GetValue<string>("UrlForPushAndroid");
            UrlForPushIOS = section.GetValue<string>("UrlForPushIOS");
        }

        public string Token { get; }

        public string KeyNameToken { get; }

        public string UrlForPushAndroid { get; }

        public string UrlForPushIOS { get; }
    }
}
