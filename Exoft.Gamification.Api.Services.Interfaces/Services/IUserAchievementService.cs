using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserAchievementService
    {
        Task AddAsync(Guid userId, Guid achievementId);
        Task DeleteAsync(Guid userAchievementsId);
        Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementsId);
        Task<ReadUserAchievementModel> GetSingleUserAchievementAsync(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
        Task<AchievementsInfo> GetAchievementsInfoByUserAsync(Guid userId);
    }
}
