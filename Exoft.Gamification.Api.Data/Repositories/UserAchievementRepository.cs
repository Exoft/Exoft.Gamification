using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserAchievementRepository : Repository<UserAchievement>, IUserAchievementRepository
    {
        public UserAchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ReturnPagingInfo<UserAchievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid UserId)
        {
            var query = IncludeAll()
                .Where(o => o.User.Id == UserId)
                .OrderByDescending(i => i.AddedTime);

            if (pagingInfo.PageSize != 0)
            {
                query.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);
            }

            var items = await query.ToListAsync();

            int itemsCount = await query.CountAsync();

            var result = new ReturnPagingInfo<UserAchievement>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = itemsCount,
                TotalPages = (int)Math.Ceiling((double)itemsCount / (pagingInfo.PageSize == 0 ? itemsCount : pagingInfo.PageSize)),
                Data = items
            };

            return result;
        }

        public async Task<int> GetCountAchievementsByThisMonthAsync(Guid userId)
        {
            return await Context.UserAchievement
                .CountAsync(i => i.AddedTime.Month == DateTime.UtcNow.Month && i.User.Id == userId);
        }

        public async Task<int> GetCountAchievementsByUserAsync(Guid userId)
        {
            return await Context.UserAchievement
                .CountAsync(o => o.User.Id == userId);
        }

        public async Task<int> GetSummaryXpByUserAsync(Guid userId)
        {
            return await IncludeAll()
                .Where(o => o.User.Id == userId)
                .SumAsync(i => i.Achievement.XP);
        }

        protected override IQueryable<UserAchievement> IncludeAll()
        {
            return DbSet
                .Include(i => i.Achievement)
                .Include(i => i.User);
        }
    }
}
