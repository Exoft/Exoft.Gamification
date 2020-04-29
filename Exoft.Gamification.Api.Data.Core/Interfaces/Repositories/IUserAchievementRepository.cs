using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IUserAchievementRepository : IRepository<UserAchievement>
    {
        Task<ReturnPagingInfo<UserAchievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);

        Task<ReturnPagingInfo<UserAchievement>> GetAllUsersByAchievementAsync(PagingInfo pagingInfo, Guid achievementId);

        Task<int> GetCountAchievementsByUserAsync(Guid userId);

        Task<int> GetCountAchievementsByThisMonthAsync(Guid userId);
    }
}
