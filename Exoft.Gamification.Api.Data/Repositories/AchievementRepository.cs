using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Achievement>> GetPagedAchievementAsync(PageInfo pageInfo)
        {
            var list = await IncludeAll().Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).ToListAsync();

            return list;
        }

        protected override IQueryable<Achievement> IncludeAll()
        {
            return DbSet.Include(i => i.Icon);
        }
    }
}
