using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface ICacheManager<T> where T : CacheObject
    {
        Task AddAsync(T entity);
        Task DeleteAsync(string key);
        Task<string> GetByKeyAsync(string key);
    }
}
