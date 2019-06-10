using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Services
{
    public class AchievementService : IAchievementService
    {
        private AchievementRepository achievementRepository;

        public AchievementService(UsersDbContext context)
        {
            achievementRepository = new AchievementRepository(context);
        }

        public async Task AddAsync(Achievement achievement)
        {
            await achievementRepository.AddAsync(achievement);
        }

        public async Task DeleteAsync(Achievement achievement)
        {
            await achievementRepository.DeleteAsync(achievement);
        }

        public async Task<Achievement> GetAchievementAsync(Guid Id)
        {
            return await achievementRepository.GetAchievementAsync(Id);
        }

        public async Task UpdateAsync(Achievement achievement)
        {
            await achievementRepository.UpdateAsync(achievement);
        }
    }
}
