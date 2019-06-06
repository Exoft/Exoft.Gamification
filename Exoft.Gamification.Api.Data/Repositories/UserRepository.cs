using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserRepository
    {
        private UsersDbContext db;

        public UserRepository(UsersDbContext context)
        {
            db = context;
        }

        public async Task AddAsync(User user)
        {
            await db.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            var entity = await db.Users.FindAsync(user.Id);
            db.Entry(entity).CurrentValues.SetValues(user);

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Id)
        {
            var entity = await db.Users.FindAsync(Id);

            if(entity != null)
            {
                db.Users.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserAsync(Guid Id)
        {
            var user = await db.Users.FirstOrDefaultAsync(item => item.Id == Id);
            var roles = await db.Set<UserRoles>().Include(r => r.Role).Where(r => r.User.Id == user.Id).ToListAsync();
            var achievements = await db.Set<UserAchievements>().Include(a => a.Achievement).Where(a => a.User.Id == user.Id).ToListAsync();
            user.Roles = roles;
            user.Achievements = achievements;

            return user;
        }

        public async Task<User> GetUserAsync(string userName)
        {
            var user = await db.Users.FirstOrDefaultAsync(item => item.UserName == userName);
            var roles = await db.Set<UserRoles>().Include(r => r.Role).Where(r => r.User.Id == user.Id).ToListAsync();
            var achievements = await db.Set<UserAchievements>().Include(a => a.Achievement).Where(a => a.User.Id == user.Id).ToListAsync();
            user.Roles = roles;
            user.Achievements = achievements;

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var list = await db.Users.ToListAsync();

            return list;
        }
    }
}
