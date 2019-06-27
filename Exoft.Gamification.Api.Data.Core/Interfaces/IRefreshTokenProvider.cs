using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IRefreshTokenProvider
    {
        Task AddAsync(RefreshToken refreshToken);
        Task DeleteAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenInfo(string token);
    }
}
