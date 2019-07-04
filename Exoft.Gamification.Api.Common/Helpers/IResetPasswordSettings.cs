using System;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IResetPasswordSettings
    {
        string ResetPasswordPage { get; }

        TimeSpan TimeToExpireSecretString { get; }
    }
}
