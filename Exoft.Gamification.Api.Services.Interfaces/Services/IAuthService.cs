using Exoft.Gamification.Api.Common.Models;
using System;

namespace Exoft.Gamification.Api.Services.Interfaces
{
    public interface IAuthService
    {
        JwtTokenModel Authenticate(string login, string password);
    }
}
