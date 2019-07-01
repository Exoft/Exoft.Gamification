using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IJwtSecret _jwtSecret;
        private readonly ICacheManager<Guid> _cacheManager;

        public RefreshTokenProvider
        (
            IJwtSecret jwtSecret,
            ICacheManager<Guid> cacheManager
        )
        {
            _jwtSecret = jwtSecret;
            _cacheManager = cacheManager;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            var cacheObject = new CacheObject<Guid>()
            {
                Key = refreshToken.Token,
                Value = refreshToken.UserId,
                TimeToExpire = _jwtSecret.TimeToExpireRefreshToken
            };

            await _cacheManager.AddAsync(cacheObject);
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            await _cacheManager.DeleteAsync(refreshToken.Token);
        }

        public async Task<RefreshToken> GetRefreshTokenInfo(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var cacheObject = await _cacheManager.GetByKeyAsync(token);
            if (cacheObject == null)
            {
                return null;
            }

            return new RefreshToken()
            {
                Token = token,
                UserId = cacheObject
            };
        }
    }
}
