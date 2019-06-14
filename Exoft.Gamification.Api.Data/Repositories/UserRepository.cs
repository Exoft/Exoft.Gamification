using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UsersDbContext context) : base(context)
        {
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
