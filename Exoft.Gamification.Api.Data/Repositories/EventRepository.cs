using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(UsersDbContext context) : base(context)
        {
        }

        public override async Task<ReturnPagingInfo<Event>> GetAllDataAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var take = pagingInfo.PageSize;
            var skip = (pagingInfo.CurrentPage - 1) * pagingInfo.PageSize;

            var query = IncludeAll()
                .OrderByDescending(s => s.CreatedTime)
                .Select(i => new
                {
                    Data = i,
                    TotalCount = IncludeAll().Count()
                });

            var entities = pagingInfo.PageSize != 0
                               ? await query.Skip(skip).Take(take).ToListAsync(cancellationToken)
                               : await query.ToListAsync(cancellationToken);

            var totalCount = entities.First().TotalCount;

            var result = new ReturnPagingInfo<Event>
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = entities.Count,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / (pagingInfo.PageSize == 0 ? totalCount : pagingInfo.PageSize)),
                Data = entities.Select(i => i.Data).ToList()
            };

            return result;
        }

        protected override IQueryable<Event> IncludeAll()
        {
            return DbSet
                .Include(s => s.User);
        }
    }
}
