using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        public ChapterRepository(UsersDbContext context) : base(context)
        {
        }

        protected override IQueryable<Chapter> IncludeAll()
        {
            return DbSet.Include(c => c.Articles);
        }
    }
}
