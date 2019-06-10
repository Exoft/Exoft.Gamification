using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    // base repository
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UsersDbContext context) : base(context)
        {

        }

        public async Task<User> GetUserAsync(Guid Id)
        {
            var user = await DbSet
                .Include(s => s.Roles)
                    .ThenInclude(s => s.Role)
                .Include(s => s.Achievements)
                    .ThenInclude(s => s.Achievement)
                .FirstOrDefaultAsync(i => i.Id == Id);

            return user;
        }

        public async Task<User> GetUserAsync(string userName)
        {
            var user = await DbSet
                .Include(s => s.Roles)
                    .ThenInclude(s => s.Role)
                .Include(s => s.Achievements)
                    .ThenInclude(s => s.Achievement)
                .FirstOrDefaultAsync(i => i.UserName == userName);

            return user;
        }
    }
}
