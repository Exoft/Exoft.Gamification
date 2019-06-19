using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IUserAchievementsRepository : IRepository<UserAchievements>
    {
        Task<UserAchievements> GetSingleUserAchievementAsync(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<UserAchievements>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
    }
}
