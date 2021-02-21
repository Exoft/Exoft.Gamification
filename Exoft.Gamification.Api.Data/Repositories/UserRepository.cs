using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GamificationDbContext context) : base(context)
        {
        }

        public override async Task<ReturnPagingInfo<User>> GetAllDataAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var take = pagingInfo.PageSize;
            var skip = (pagingInfo.CurrentPage - 1) * pagingInfo.PageSize;

            var query = IncludeAll()
                .OrderBy(s => s.Achievements.Sum(a => a.Achievement.XP))
                .Select(i => new
                {
                    Data = i,
                    TotalCount = IncludeAll().Count()
                });

            var entities = pagingInfo.PageSize != 0
                               ? await query.Skip(skip).Take(take).ToListAsync(cancellationToken)
                               : await query.ToListAsync(cancellationToken);

            var totalCount = entities.First().TotalCount;

            var result = new ReturnPagingInfo<User>
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = entities.Count,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / (pagingInfo.PageSize == 0 ? totalCount : pagingInfo.PageSize)),
                Data = entities.Select(i => i.Data).ToList()
            };

            return result;
        }

        public async Task<bool> DoesEmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            var result = await IncludeAll().AnyAsync(i => i.Email == email, cancellationToken);

            return result;
        }
        
        public async Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await IncludeAll().SingleOrDefaultAsync(i => i.UserName == userName, cancellationToken);

            return user;
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await IncludeAll().SingleOrDefaultAsync(i => i.Email == email, cancellationToken);

            return user;
        }

        public async Task<ICollection<string>> GetAdminsEmailsAsync(CancellationToken cancellationToken)
        {
            var list = await Context.UserRoles
                .Where(i => i.Role.Name == GamificationRole.Admin)
                .Select(i => i.User.Email)
                .ToListAsync(cancellationToken);

            return list;
        }

        protected override IQueryable<User> IncludeAll()
        {
            return DbSet
                .Include(s => s.Roles)
                    .ThenInclude(s => s.Role)
                .Include(s => s.Achievements)
                    .ThenInclude(s => s.Achievement);
        }
    }
}
