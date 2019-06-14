using Exoft.Gamification.Api.Common.Models;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IAuthService
    {
        Task<JwtTokenModel> Authenticate(string login, string password);
    }
}
