using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserAchievementService
    {
        Task AddAsync(Guid userId, Guid achievementId);
        Task DeleteAsync(Guid userAchievementsId);
        Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementsId);
        Task<ReadUserAchievementModel> GetSingleUserAchievementAsync(Guid userAchievementId);
        Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
        Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId);
        Task AddOrUpdateAchievementsRangeAsync(Guid userId, IEnumerable<Guid> models);
        Task<ReadUserAchievementModel> GetUserAchievementsByIdsAsync(Guid[] achievemetnsIds);
    }
}
