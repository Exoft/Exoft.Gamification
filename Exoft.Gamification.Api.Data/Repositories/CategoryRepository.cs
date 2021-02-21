using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(UsersDbContext context)
            : base(context)
        {
        }

        public async Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await IncludeAll().SingleOrDefaultAsync(i => i.Name == name, cancellationToken);
        }

        protected override IQueryable<Category> IncludeAll()
        {
            return DbSet;
        }
    }
}
