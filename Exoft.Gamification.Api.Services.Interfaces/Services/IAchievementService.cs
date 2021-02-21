using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAchievementService
    {
        Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model, CancellationToken cancellationToken);

        Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid id, CancellationToken cancellationToken);

        Task DeleteAchievementAsync(Guid id, CancellationToken cancellationToken);

        Task<ReadAchievementModel> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);
    }
}
