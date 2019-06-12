using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<ICollection<Achievement>> GetPagedAchievementAsync(PageInfo pageInfo);
    }
}
