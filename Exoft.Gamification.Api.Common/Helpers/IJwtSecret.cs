namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        byte[] Secret { get; }
        int SecondsToExpireToken { get; }
        int SecondsToExpireRefreshToken { get; }
    }
}
