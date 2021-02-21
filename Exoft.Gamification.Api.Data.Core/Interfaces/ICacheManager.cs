using System.Threading;
using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface ICacheManager<T>
    {
        Task AddAsync(CacheObject<T> entity, CancellationToken cancellationToken);
        Task DeleteAsync(string key, CancellationToken cancellationToken);
        Task<T> GetByKeyAsync(string key, CancellationToken cancellationToken);
    }
}
