using System;
using System.Linq;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class RequestOrderRepository : Repository<RequestOrder>, IRequestOrderRepository
    {
        public RequestOrderRepository(UsersDbContext context)
            : base(context)
        {
        }

        public override async Task<ReturnPagingInfo<RequestOrder>> GetAllDataAsync(PagingInfo pagingInfo)
        {
            var query = IncludeAll()
                .Where(i => i.Status == GamificationEnums.RequestStatus.Pending)
                .OrderBy(s => s.Id)
                .AsQueryable();
            if (pagingInfo.PageSize != 0)
            {
                query = query.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);
            }

            var items = await query.ToListAsync();

            int allItemsCount = await IncludeAll().CountAsync();

            var result = new ReturnPagingInfo<RequestOrder>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = allItemsCount,
                TotalPages = (int)Math.Ceiling((double)allItemsCount / (pagingInfo.PageSize == 0 ? allItemsCount : pagingInfo.PageSize)),
                Data = items
            };

            return result;
        }

        protected override IQueryable<RequestOrder> IncludeAll()
        {
            return DbSet;
        }
    }
}
