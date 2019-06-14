using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {

        public AchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ReturnPagingInfo<Achievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId)
        {
            var list = Context.UserAchievements
                .Where(o => o.User.Id == userId)
                .Select(i => i.Achievement)
                .OrderBy(i => i.XP);
            
            var items = list
                .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToList();

            int listCount = await list.CountAsync();

            var result = new ReturnPagingInfo<Achievement>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = listCount,
                TotalPages = (int)Math.Ceiling((double)listCount / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }

        protected override IQueryable<Achievement> IncludeAll()
        {
            return DbSet
                .Include(i => i.Icon);
        }

        public async Task<Achievement> GetSingleUserAchievementAsync(Guid userId, Guid achievementId)
        {
            return await Context.UserAchievements
                .Where(o => o.User.Id == userId && o.Achievement.Id == achievementId)
                .Select(i => i.Achievement)
                .SingleOrDefaultAsync();
        }

    }
}
