using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;

namespace Exoft.Gamification.Api.Services.Helpers
{
    public class NotAllowedResponse : IResponse
    {
        public NotAllowedResponse(string message)
        {
            Type = GamificationEnums.ResponseType.NotAllowed;

            Message = message;
        }

        public GamificationEnums.ResponseType Type { get; }

        public string Message { get; }
    }
}
