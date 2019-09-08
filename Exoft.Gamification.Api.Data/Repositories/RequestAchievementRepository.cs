using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class RequestAchievementRepository : Repository<RequestAchievement>, IRequestAchievementRepository
    {
        public RequestAchievementRepository(UsersDbContext context) : base(context)
        {
        }

        protected override IQueryable<RequestAchievement> IncludeAll()
        {
            return DbSet;
        }

        public async override Task<ReturnPagingInfo<RequestAchievement>> GetAllDataAsync(PagingInfo pagingInfo)
        {

            var items = await IncludeAll()
            .OrderBy(s => s.Id)
            .Include(a => a.Achievement)
            .Include(u => u.User)
            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
            .Take(pagingInfo.PageSize)
            .ToListAsync();

            int allItemsCount = await IncludeAll().CountAsync();

            var result = new ReturnPagingInfo<RequestAchievement>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = allItemsCount,
                TotalPages = (int)Math.Ceiling((double)allItemsCount / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }
    }
}
