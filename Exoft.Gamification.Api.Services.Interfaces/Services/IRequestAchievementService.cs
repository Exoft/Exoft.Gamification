using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRequestAchievementService
    {
        Task<IResponse> AddAsync(RequestAchievementModel model, Guid userId);
        Task<RequestAchievement> GetByIdAsync(Guid id);
        Task<IEnumerable<RequestAchievement>> GetAllAsync();
        Task ApproveAchievementRequestAsync(Guid id);
        Task DeleteAsync(RequestAchievement achievementRequest);
    }
}
