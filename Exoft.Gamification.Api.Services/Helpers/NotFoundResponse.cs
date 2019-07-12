using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;

namespace Exoft.Gamification.Api.Services.Helpers
{
    public class NotFoundResponse : IResponse
    {
        public NotFoundResponse(string message)
        {
            Type = GamificationEnums.ResponseType.NotFound;

            Message = message;
        }

        public GamificationEnums.ResponseType Type { get; }

        public string Message { get; }
    }
}
