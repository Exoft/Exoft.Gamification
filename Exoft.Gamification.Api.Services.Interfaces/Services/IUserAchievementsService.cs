using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserAchievementsService
    {
        Task AddAsync(Guid userId, Guid achievementId);
        Task DeleteAsync(Guid userAchievementsId);
        Task<ReadAchievementModel> GetSingleUserAchievementAsync(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId);
    }
}
