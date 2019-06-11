using Exoft.Gamification.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task AddAchievementAsync(InAchievementModel model);
        void UpdateAchievement(InAchievementModel model, Guid Id);
        void DeleteAchievement(Guid Id);
        Task<OutAchievementModel> GetAchievementByIdAsync(Guid Id);
        Task<ICollection<OutAchievementModel>> GetAchievementsDyUserAsync(Guid Id);
        Task<ICollection<OutAchievementModel>> GetAllAsync();
    }
}
