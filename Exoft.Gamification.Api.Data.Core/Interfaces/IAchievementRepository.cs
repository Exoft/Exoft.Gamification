using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<Achievement> IsUserHaveAchievementAsync(Guid userId, Guid achievementId);
        Task<ReturnPagingInfo<Achievement>> GetPagedAchievementByUserAsync(PagingInfo pagingInfo, Guid userId);
    }
}
