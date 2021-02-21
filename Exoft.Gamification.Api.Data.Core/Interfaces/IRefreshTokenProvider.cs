using System.Threading;
using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IRefreshTokenProvider
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<RefreshToken> GetRefreshTokenInfo(string token, CancellationToken cancellationToken);
    }
}
