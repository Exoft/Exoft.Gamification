using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAuthService
    {
        Task<JwtTokenModel> AuthenticateAsync(string login, string password);

        Task<JwtTokenModel> AuthenticateByEmailAsync(string email, string password);

        Task<JwtTokenModel> RefreshTokenAsync(string refreshToken);

        Task<IResponse> SendForgotPasswordAsync(string email, Uri resetPasswordPageLink);

        Task<IResponse> ResetPasswordAsync(string secretString, string newPassword);
    }
}
