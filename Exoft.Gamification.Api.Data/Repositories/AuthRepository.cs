using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IJwtSecret _jwtSecret;

        public AuthRepository
        (
            IMemoryCache cache,
            IJwtSecret jwtSecret
        )
        {
            _cache = cache;
            _jwtSecret = jwtSecret;
        }
        
        public void Add(RefreshToken entity)
        {
            _cache.Set(entity.UserId, entity, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_jwtSecret.SecondsToExpireRefreshToken)
            });
        }

        public void Delete(RefreshToken entity)
        {
            _cache.Remove(entity.UserId);
        }

        public RefreshToken GetByUserId(Guid userId)
        {
            var refreshToken = _cache.Get(userId) as RefreshToken;

            return refreshToken;
        }
    }
}
