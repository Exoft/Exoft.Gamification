using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class CacheManager<T> : ICacheManager<T>
    {
        protected readonly IDistributedCache _cache;

        public CacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(CacheObject<T> entity)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = entity.TimeToExpire
            };

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, entity.Value);

                await _cache.SetAsync(entity.Key, memoryStream.ToArray(), options);
            }
        }

        public async Task DeleteAsync(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            await _cache.RemoveAsync(key);
        }

        public async Task<T> GetByKeyAsync(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            var value = await _cache.GetAsync(key);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await memoryStream.WriteAsync(value, 0, value.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}
