using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model);
        Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid Id);
        Task DeleteAchievementAsync(Guid Id);
        Task<ReadAchievementModel> GetAchievementByIdAsync(Guid Id);
        Task<ReadAchievementModel> DoesUserHaveAchievement(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsAsync(PagingInfo pagingInfo);
        Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid UserId);
    }
}
