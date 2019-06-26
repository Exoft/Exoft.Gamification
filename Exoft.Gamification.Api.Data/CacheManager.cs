using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public abstract class CacheManager<T> : ICacheManager<T>
    {
        protected readonly IDistributedCache _cache;

        public CacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public abstract Task AddAsync(T entity);

        public abstract Task DeleteAsync(T entity);

        public abstract Task<T> GetByKeyAsync(string key);
    }
}
