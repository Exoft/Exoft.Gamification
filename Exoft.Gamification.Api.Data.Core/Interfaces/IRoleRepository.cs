using Exoft.Gamification.Api.Data.Core.Entities;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetRoleByNameAsync(string name);
    }
}
