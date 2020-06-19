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
            var take = pagingInfo.PageSize;
            var skip = (pagingInfo.CurrentPage - 1) * pagingInfo.PageSize;

            var query = IncludeAll()
                .Where(s => s.Status == GamificationEnums.RequestStatus.Pending)
                .OrderBy(s => s.Id)
                .Select(i => new
                {
                    Data = i,
                    TotalCount = IncludeAll().Count()
                });

            var entities = pagingInfo.PageSize != 0
                               ? await query.Skip(skip).Take(take).ToListAsync()
                               : await query.ToListAsync();

            var totalCount = entities.FirstOrDefault()?.TotalCount ?? 0;

            var result = new ReturnPagingInfo<RequestOrder>
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = entities.Count,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / (pagingInfo.PageSize == 0 ? totalCount : pagingInfo.PageSize)),
                Data = entities.Select(i => i.Data).ToList()
            };

            return result;
        }

        protected override IQueryable<RequestOrder> IncludeAll()
        {
            return DbSet;
        }
    }
}
