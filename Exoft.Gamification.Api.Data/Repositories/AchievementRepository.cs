﻿using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AchievementRepository : Repository<Achievement>, IAchievementRepository
    {

        public AchievementRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<Achievement> GetAchievementByNameAsync(string name)
        {
            var achievement = await IncludeAll().SingleOrDefaultAsync(i => i.Name == name);

            return achievement;
        }

        protected override IQueryable<Achievement> IncludeAll()
        {
            return DbSet;
        }
    }
}
