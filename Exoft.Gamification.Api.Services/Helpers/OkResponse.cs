using Exoft.Gamification.Api.Services.Interfaces;

namespace Exoft.Gamification.Api.Services.Helpers
{
    public class OkResponse : IResponse
    {
        public OkResponse()
        {
            Success = true;
        }

        public bool Success { get; }

        public string Error { get; }
    }
}
