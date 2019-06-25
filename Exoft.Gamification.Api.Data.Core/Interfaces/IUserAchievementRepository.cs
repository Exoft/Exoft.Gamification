using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IUserAchievementRepository : IRepository<UserAchievement>
    {
        Task<UserAchievement> GetSingleUserAchievementAsync(Guid userAchievementId);
        Task<ReturnPagingInfo<UserAchievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
        Task<int> GetCountAchievementsByUserAsync(Guid userId);
        Task<int> GetSummaryXpByUserAsync(Guid userId);
        Task<int> GetCountAchievementsByThisMonthAsync(Guid userId);
    }
}
