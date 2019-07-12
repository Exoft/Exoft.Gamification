using System;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IResetPasswordSettings
    {
        TimeSpan TimeToExpireSecretString { get; }
    }
}
