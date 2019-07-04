using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces
{
    public interface IResponse
    {
        GamificationEnums.ResponseType Type { get; }

        string Message { get; }
    }
}
