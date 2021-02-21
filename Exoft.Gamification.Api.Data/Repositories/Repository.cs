using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        protected Repository(GamificationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            DbSet = context.Set<T>();
        }

        protected DbSet<T> DbSet { get; }

        protected GamificationDbContext Context { get; }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await IncludeAll().SingleOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public virtual Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return AddInternalAsync(entity, cancellationToken);
        }

        private async Task AddInternalAsync(T entity, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Remove(entity);
        }

        public virtual async Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var take = pagingInfo.PageSize;
            var skip = (pagingInfo.CurrentPage - 1) * pagingInfo.PageSize;

            var query = IncludeAll()
                .OrderBy(s => s.Id)
                .Select(i => new
                {
                    Data = i, 
                    TotalCount = IncludeAll().Count()
                });

            var entities = pagingInfo.PageSize != 0
                               ? await query.Skip(skip).Take(take).ToListAsync(cancellationToken)
                               : await query.ToListAsync(cancellationToken);

            var totalCount = entities.First().TotalCount;

            var result = new ReturnPagingInfo<T>
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = entities.Count,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / (pagingInfo.PageSize == 0 ? totalCount : pagingInfo.PageSize)),
                Data = entities.Select(i => i.Data).ToList()
            };

            return result;
        }

        protected abstract IQueryable<T> IncludeAll();
    }
}
