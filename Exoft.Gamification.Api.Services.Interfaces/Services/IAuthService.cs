using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAuthService
    {
        Task<JwtTokenModel> AuthenticateAsync(string login, string password, CancellationToken cancellationToken);

        Task<JwtTokenModel> AuthenticateByEmailAsync(string email, string password, CancellationToken cancellationToken);

        Task<JwtTokenModel> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

        Task<IResponse> SendForgotPasswordAsync(string email, Uri resetPasswordPageLink, CancellationToken cancellationToken);

        Task<IResponse> ResetPasswordAsync(string secretString, string newPassword, CancellationToken cancellationToken);
    }
}
