using Exoft.Gamification.Api.Data.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Repositories
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<ICollection<Achievement>> GetAllAsync();
    }
}
