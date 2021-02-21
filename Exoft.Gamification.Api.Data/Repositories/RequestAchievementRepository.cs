using System.Linq;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class RequestAchievementRepository : Repository<RequestAchievement>, IRequestAchievementRepository
    {
        public RequestAchievementRepository(GamificationDbContext context) : base(context)
        {
        }

        protected override IQueryable<RequestAchievement> IncludeAll()
        {
            return DbSet;
        }
    }
}
