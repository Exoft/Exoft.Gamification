using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserAchievementsRepository : Repository<UserAchievements>, IUserAchievementsRepository
    {
        public UserAchievementsRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ReturnPagingInfo<UserAchievements>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid UserId)
        {
            var list = IncludeAll()
                .Where(o => o.User.Id == UserId)
                .Select(i => i)
                .OrderBy(i => i.Achievement.XP);

            var items = list
                .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToList();

            int listCount = await list.CountAsync();

            var result = new ReturnPagingInfo<UserAchievements>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = listCount,
                TotalPages = (int)Math.Ceiling((double)listCount / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }

        public async Task<UserAchievements> GetSingleUserAchievementAsync(Guid userId, Guid achievementId)
        {
            return await IncludeAll()
                .Where(o => o.User.Id == userId && o.Achievement.Id == achievementId)
                .Select(i => i)
                .SingleOrDefaultAsync();
        }

        protected override IQueryable<UserAchievements> IncludeAll()
        {
            return DbSet
                .Include(i => i.Achievement)
                .Include(i => i.User);
        }
    }
}
