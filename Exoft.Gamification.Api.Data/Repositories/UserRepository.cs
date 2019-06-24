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

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var result = await IncludeAll().AnyAsync(i => i.Email == email);

            return result;
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
