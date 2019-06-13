using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model);
        Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid Id);
        Task DeleteAchievementAsync(Guid Id);
        Task<ReadAchievementModel> GetAchievementByIdAsync(Guid Id);
        Task<ReadAchievementModel> IsUserHaveAchievement(Guid userId, Guid achievementId);
        Task<ReturnPagingModel<ReadAchievementModel>> GetPagedAchievementAsync(InputPagingModel pagingModel);
        Task<ReturnPagingModel<ReadAchievementModel>> GetPagedAchievementByUserAsync(InputPagingModel pagingModel, Guid UserId);
    }
}
