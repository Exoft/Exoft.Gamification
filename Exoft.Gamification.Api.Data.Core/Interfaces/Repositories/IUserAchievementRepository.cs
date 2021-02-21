using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IUserAchievementRepository : IRepository<UserAchievement>
    {
        Task<ReturnPagingInfo<UserAchievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<UserAchievement>> GetAllUsersByAchievementAsync(PagingInfo pagingInfo, Guid achievementId, CancellationToken cancellationToken);

        Task<int> GetCountAchievementsByUserAsync(Guid userId, CancellationToken cancellationToken);

        Task<int> GetCountAchievementsByThisMonthAsync(Guid userId, CancellationToken cancellationToken);
    }
}
