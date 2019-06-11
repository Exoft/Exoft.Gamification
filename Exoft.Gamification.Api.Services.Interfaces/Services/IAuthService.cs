using Exoft.Gamification.Api.Common.Models;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<JwtTokenModel> Authenticate(string login, string password);
    }
}
