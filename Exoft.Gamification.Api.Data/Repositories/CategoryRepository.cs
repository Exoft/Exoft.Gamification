using System.Linq;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(UsersDbContext context)
            : base(context)
        {
        }

        protected override IQueryable<Category> IncludeAll()
        {
            return DbSet;
        }
    }
}
