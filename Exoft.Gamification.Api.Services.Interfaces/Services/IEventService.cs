using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IEventService
    {
        Task<ReturnPagingInfo<EventModel>> GetAllEventAsync(PagingInfo pagingInfo);
    }
}
