using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model);

        Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid id);

        Task DeleteAchievementAsync(Guid id);

        Task<ReadAchievementModel> GetAchievementByIdAsync(Guid id);

        Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsAsync(PagingInfo pagingInfo);
    }
}
