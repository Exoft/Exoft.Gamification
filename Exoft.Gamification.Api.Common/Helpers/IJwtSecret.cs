namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        byte[] Secret { get; }
    }
}
