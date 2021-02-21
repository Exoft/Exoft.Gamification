using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(GamificationDbContext context) : base(context)
        {
        }

        public async Task<Role> GetRoleByNameAsync(string name, CancellationToken cancellationToken)
        {
            var role = await IncludeAll().SingleOrDefaultAsync(i => i.Name == name, cancellationToken);

            return role;
        }

        protected override IQueryable<Role> IncludeAll()
        {
            return DbSet;
        }
    }
}
