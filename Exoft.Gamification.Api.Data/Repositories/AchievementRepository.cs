using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Achievement>> GetAllAsync()
        {
            return await IncludeAll().ToListAsync();
        }

        protected override IQueryable<Achievement> IncludeAll()
        {
            return DbSet.Include(i => i.Icon);
        }
    }
}
