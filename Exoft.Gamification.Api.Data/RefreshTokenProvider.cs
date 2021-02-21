using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using System;
using System.Threading;
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

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var cacheObject = new CacheObject<Guid>
            {
                Key = refreshToken.Token,
                Value = refreshToken.UserId,
                TimeToExpire = _jwtSecret.TimeToExpireRefreshToken
            };

            await _cacheManager.AddAsync(cacheObject, cancellationToken);
        }

        public async Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _cacheManager.DeleteAsync(refreshToken.Token, cancellationToken);
        }

        public async Task<RefreshToken> GetRefreshTokenInfo(string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var cacheObject = await _cacheManager.GetByKeyAsync(token, cancellationToken);
            if (cacheObject == default(Guid))
            {
                return null;
            }

            return new RefreshToken
            {
                Token = token,
                UserId = cacheObject
            };
        }
    }
}
