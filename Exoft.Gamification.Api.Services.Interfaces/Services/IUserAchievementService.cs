using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserAchievementService
    {
        Task AddAsync(Guid userId, Guid achievementId, CancellationToken cancellationToken);

        Task DeleteAsync(Guid userAchievementsId, CancellationToken cancellationToken);

        Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementsId, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(
            PagingInfo pagingInfo,
            Guid userId, 
            CancellationToken cancellationToken);

        Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId, CancellationToken cancellationToken);

        Task ChangeUserAchievementsAsync(AssignAchievementsToUserModel model, Guid userId, CancellationToken cancellationToken);
    }
}
