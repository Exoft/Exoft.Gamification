using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<Achievement> GetSingleUserAchievementAsync(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<Achievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
    }
}
