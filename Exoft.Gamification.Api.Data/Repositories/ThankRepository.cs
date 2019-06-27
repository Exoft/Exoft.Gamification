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

        public async Task<Thank> GetThankAsync(Guid toUserId)
        {
            return await IncludeAll()
                .SingleOrDefaultAsync(i => i.ToUserId == toUserId);
        }

        protected override IQueryable<Thank> IncludeAll()
        {
            return DbSet;
        }
    }
}
