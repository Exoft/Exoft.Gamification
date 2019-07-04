namespace Exoft.Gamification.Api.Services.Interfaces
{
    public interface IResponse
    {
        bool Success { get; }

        string Error { get; }
    }
}
