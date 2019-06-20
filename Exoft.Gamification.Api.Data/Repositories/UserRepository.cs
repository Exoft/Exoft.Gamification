using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UsersDbContext context) : base(context)
        {
        }

        public override Task AddAsync(User entity)
        {
            entity.Password = entity.Password.GetMD5Hash();
            return base.AddAsync(entity);
        }

        public override async Task<ReturnPagingInfo<User>> GetAllDataAsync(PagingInfo pagingInfo)
        {
            var items = await IncludeAll()
                .OrderByDescending(s => s.XP)
                .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToListAsync();

            int allItemsCount = await IncludeAll().CountAsync();

            var result = new ReturnPagingInfo<User>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = allItemsCount,
                TotalPages = (int)Math.Ceiling((double)allItemsCount / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            var user = await IncludeAll().SingleOrDefaultAsync(i => i.UserName == userName);

            return user;
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
