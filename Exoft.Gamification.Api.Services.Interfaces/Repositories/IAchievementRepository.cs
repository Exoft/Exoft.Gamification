using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Repositories
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<Achievement> GetAchievementAsync(Guid Id);
    }
}
