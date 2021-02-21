using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class FileRepository : Repository<File>, IFileRepository
    {
        public FileRepository(GamificationDbContext context) : base(context)
        {
        }

        public async Task Delete(Guid fileId, CancellationToken cancellationToken)
        {
            await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Files WHERE Id = {fileId}", cancellationToken);
        }

        protected override IQueryable<File> IncludeAll()
        {
            return DbSet;
        }
    }
}
