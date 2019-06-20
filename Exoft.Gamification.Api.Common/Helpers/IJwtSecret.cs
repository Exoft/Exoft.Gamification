namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        byte[] Secret { get; }
        string ExpireToken { get; }
        string ExpireRefreshToken { get; }
    }
}
