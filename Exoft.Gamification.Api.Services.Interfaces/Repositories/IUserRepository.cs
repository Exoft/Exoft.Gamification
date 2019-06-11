using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName);
    }
}
