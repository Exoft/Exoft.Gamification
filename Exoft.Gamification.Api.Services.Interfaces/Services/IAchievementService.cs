using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model);
        ReadAchievementModel UpdateAchievement(UpdateAchievementModel model, Guid Id);
        void DeleteAchievementAsync(Guid Id);
        Task<ReadAchievementModel> GetAchievementByIdAsync(Guid Id);
        Task<ICollection<ReadAchievementModel>> GetUserAchievementsAsync(Guid Id);
        Task<ICollection<ReadAchievementModel>> GetPagedAchievement(PageInfo pageInfo);
        Task SaveChangesAsync();
    }
}
