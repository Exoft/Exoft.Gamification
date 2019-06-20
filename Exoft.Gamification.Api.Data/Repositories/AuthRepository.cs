using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AuthRepository : Repository<RefreshToken>, IAuthRepository
    {
        public AuthRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken> GetByUserIdAsync(Guid userId)
        {
            var refreshToken = await IncludeAll().FirstOrDefaultAsync(i => i.User.Id == userId);

            return refreshToken;
        }

        protected override IQueryable<RefreshToken> IncludeAll()
        {
            return DbSet
                .Include(i => i.User);
        }
    }
}
