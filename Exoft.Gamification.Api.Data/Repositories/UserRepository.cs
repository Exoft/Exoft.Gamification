using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Achievement>> GetAchievementsByUserAsync(Guid Id)
        {
            var user = await GetByIdAsync(Id);

            return user.Achievements.Select(i => i.Achievement).ToList();
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
