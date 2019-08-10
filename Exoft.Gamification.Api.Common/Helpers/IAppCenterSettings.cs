using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IAppCenterSettings
    {
        string Token { get; }
        string KeyNameToken { get; }
        string UrlForPushAndroid { get; }
        string UrlForPushIOS { get; }

    }
}
