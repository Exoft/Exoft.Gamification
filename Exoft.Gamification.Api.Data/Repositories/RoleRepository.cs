using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(UsersDbContext context) : base(context)
        {
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            var role = await IncludeAll().SingleOrDefaultAsync(i => i.Name == name);

            return role;
        }

        protected override IQueryable<Role> IncludeAll()
        {
            return DbSet;
        }
    }
}
