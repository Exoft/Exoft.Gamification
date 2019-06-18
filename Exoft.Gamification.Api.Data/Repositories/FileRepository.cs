using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using System.Linq;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class FileRepository : Repository<File>, IFileRepository
    {
        public FileRepository(UsersDbContext context) : base(context)
        {
        }

        protected override IQueryable<File> IncludeAll()
        {
            return DbSet;
        }
    }
}
