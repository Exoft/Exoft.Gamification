using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IAchievementRepository : IRepository<Achievement>
    {
        Task<Achievement> GetAchievementByNameAsync(string name);
    }
}
