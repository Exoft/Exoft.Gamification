using Exoft.Gamification.Api.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var user = await db.Users.SingleOrDefaultAsync(item => item.Id == Id);
            return user;
        }

        public async Task<User> GetUserAsync(string userName)
        {
            var user = await db.Users.SingleOrDefaultAsync(item => item.UserName == userName);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var list = await db.Users.ToListAsync();

            return list;
        }
    }
}
