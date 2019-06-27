using System;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        byte[] Secret { get; }
        TimeSpan TimeToExpireToken { get; }
        TimeSpan TimeToExpireRefreshToken { get; }
    }
}
