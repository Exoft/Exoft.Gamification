using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class ThankRepository : Repository<Thank>, IThankRepository
    {
        public ThankRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<Thank> GetLastThankAsync(Guid toUserId)
        {
            return await IncludeAll()
                .OrderByDescending(i => i.AddedTime)
                .FirstOrDefaultAsync(i => i.ToUserId == toUserId);
        }

        protected override IQueryable<Thank> IncludeAll()
        {
            return DbSet
                .Include(i => i.FromUser);
        }
    }
}
