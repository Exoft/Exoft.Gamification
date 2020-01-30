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
        Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
        Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId);
        Task ChangeUserAchievementsAsync(AssignAchievementsToUserModel model, Guid userId);
    }
}
