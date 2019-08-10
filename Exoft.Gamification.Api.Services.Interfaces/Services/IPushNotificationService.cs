using Exoft.Gamification.Api.Common.Models;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IPushNotificationService
    {
        Task<(OSType type, bool isSend)> SendNotification(PushRequestModel pushRequestModel);
    }
}
