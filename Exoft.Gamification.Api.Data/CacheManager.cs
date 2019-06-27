using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class CacheManager<T> : ICacheManager<T> where T : CacheObject
    {
        protected readonly IDistributedCache _cache;

        public CacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(T entity)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = entity.TimeToExpire
            };
            
            await _cache.SetStringAsync(entity.Key, entity.Value, options);
        }

        public async Task DeleteAsync(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            await _cache.RemoveAsync(key);
        }

        public async Task<string> GetByKeyAsync(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                return null;
            }

            return await _cache.GetStringAsync(key);
        }
    }
}
