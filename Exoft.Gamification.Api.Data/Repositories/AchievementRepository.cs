using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(UsersDbContext context) : base(context)
        {

        }

        public async Task<Achievement> GetAchievementAsync(Guid Id)
        {
            var achievement = await DbSet.FirstOrDefaultAsync(i => i.Id == Id);

            return achievement;
        }
    }
}
