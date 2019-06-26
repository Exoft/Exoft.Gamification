using System;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        byte[] Secret { get; }
        TimeSpan SecondsToExpireToken { get; }
        TimeSpan SecondsToExpireRefreshToken { get; }
    }
}
