using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface ICacheManager<T>
    {
        Task AddAsync(CacheObject<T> entity);
        Task DeleteAsync(string key);
        Task<T> GetByKeyAsync(string key);
    }
}
