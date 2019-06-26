using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class AuthCacheManager : CacheManager<RefreshToken>, IAuthCacheManager
    {
        private readonly IJwtSecret _jwtSecret;

        public AuthCacheManager
        (
            IJwtSecret jwtSecret,
            IDistributedCache cache
        ) : base(cache)
        {
            _jwtSecret = jwtSecret;
        }

        public override async Task AddAsync(RefreshToken entity)
        {
            await _cache.SetStringAsync(entity.Token, entity.UserId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _jwtSecret.SecondsToExpireRefreshToken
            });
        }

        public override async Task DeleteAsync(RefreshToken entity)
        {
            await _cache.RemoveAsync(entity.Token);
        }

        public override async Task<RefreshToken> GetByKeyAsync(string key)
        {
            if(key == null)
            {
                return null;
            }

            var userId = await _cache.GetStringAsync(key);
            if(userId == null)
            {
                return null;
            }

            var refreshToken = new RefreshToken()
            {
                Token = key,
                UserId = Guid.Parse(userId)
            };

            return refreshToken;
        }
    }
}
