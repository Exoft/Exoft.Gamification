using System;
using System.Linq;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserAchievementRepository : Repository<UserAchievement>, IUserAchievementRepository
    {
        public UserAchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ReturnPagingInfo<UserAchievement>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId)
        {
            var take = pagingInfo.PageSize;
            var skip = (pagingInfo.CurrentPage - 1) * pagingInfo.PageSize;

            var query = IncludeAll()
                .Where(i => i.User.Id == userId)
                .OrderByDescending(s => s.AddedTime)
                .Select(i => new
                {
                    Data = i,
                    TotalCount = IncludeAll().Count()
                });

            var entities = pagingInfo.PageSize != 0
                               ? await query.Skip(skip).Take(take).ToListAsync()
                               : await query.ToListAsync();

            var totalCount = entities.First().TotalCount;

            var result = new ReturnPagingInfo<UserAchievement>
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = entities.Count,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / (pagingInfo.PageSize == 0 ? totalCount : pagingInfo.PageSize)),
                Data = entities.Select(i => i.Data).ToList()
            };

            return result;
        }

        public async Task<ReturnPagingInfo<UserAchievement>> GetAllUsersByAchievementAsync(PagingInfo pagingInfo, Guid achievementId)
        {
            var query = IncludeAll()
                .Where(o => o.Achievement.Id == achievementId)
                .AsQueryable();

            if (pagingInfo.PageSize != 0)
            {
                query = query.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);
            }

            var items = await query.ToListAsync();

            //// todo: use this code after update core >= 3.0.0
            ////var result = new ReturnPagingInfo<UserAchievement>()
            ////{
            ////    CurrentPage = pagingInfo.CurrentPage,
            ////    PageSize = items.Count,
            ////    TotalItems = query.Count(),
            ////    TotalPages = (int)Math.Ceiling((double)query.Count() / (pagingInfo.PageSize == 0 ? query.Count() : pagingInfo.PageSize)),
            ////    Data = items
            ////};

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

        protected override IQueryable<UserAchievement> IncludeAll()
        {
            return DbSet
                .Include(i => i.Achievement)
                .Include(i => i.User);
        }
    }
}
