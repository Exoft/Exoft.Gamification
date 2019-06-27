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
        private readonly ICacheManager<CacheObject> _cacheManager;

        public RefreshTokenProvider
        (
            IJwtSecret jwtSecret,
            ICacheManager<CacheObject> cacheManager
        )
        {
            _jwtSecret = jwtSecret;
            _cacheManager = cacheManager;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            var cacheObject = new CacheObject()
            {
                Key = refreshToken.Token,
                Value = refreshToken.UserId.ToString(),
                TimeToExpire = _jwtSecret.TimeToExpireRefreshToken
            };

            await _cacheManager.AddAsync(cacheObject);
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            var cacheObject = await _cacheManager.GetByKeyAsync(refreshToken.Token);

            await _cacheManager.DeleteAsync(cacheObject);
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
                UserId = Guid.Parse(cacheObject)
            };
        }
    }
}
