using Exoft.Gamification.Api.Services.Interfaces;

namespace Exoft.Gamification.Api.Services.Helpers
{
    public class NotFoundResponse : IResponse
    {
        public NotFoundResponse(string message)
        {
            Success = false;

            Error = message;
        }

        public bool Success { get; }

        public string Error { get; }
    }
}
