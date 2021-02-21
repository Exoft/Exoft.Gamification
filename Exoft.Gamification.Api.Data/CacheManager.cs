using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class CacheManager<T> : ICacheManager<T>
    {
        private readonly IDistributedCache _cache;

        public CacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(CacheObject<T> entity, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = entity.TimeToExpire
            };
            
            var binaryFormatter = new BinaryFormatter();
            await using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, entity.Value);

                await _cache.SetAsync(entity.Key, memoryStream.ToArray(), options, cancellationToken);
            }
        }

        public Task DeleteAsync(string key, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return DeleteInternalAsync(key, cancellationToken);
        }

        private async Task DeleteInternalAsync(string key, CancellationToken cancellationToken)
        {
            await  _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<T> GetByKeyAsync(string key, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(key))
            {
                return default;
            }

            var value = await _cache.GetAsync(key, cancellationToken);
            if(value == null)
            {
                return default;
            }

            var binaryFormatter = new BinaryFormatter();
            await using (var memoryStream = new MemoryStream())
            {
                await memoryStream.WriteAsync(value, 0, value.Length, cancellationToken);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}
