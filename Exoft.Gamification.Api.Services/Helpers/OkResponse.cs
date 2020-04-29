using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;

namespace Exoft.Gamification.Api.Services.Helpers
{
    public class OkResponse : IResponse
    {
        public OkResponse()
        {
            Type = GamificationEnums.ResponseType.Ok;

            Message = string.Empty;
        }

        public GamificationEnums.ResponseType Type { get; }

        public string Message { get; }
    }
}
