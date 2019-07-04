using Exoft.Gamification.Api.Common.Models;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAuthService
    {
        Task<JwtTokenModel> AuthenticateAsync(string login, string password);
        Task<JwtTokenModel> RefreshTokenAsync(string refreshToken);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string secretString, string newPassword);
    }
}
