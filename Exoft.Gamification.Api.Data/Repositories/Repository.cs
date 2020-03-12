using System;
using System.Linq;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        protected Repository(UsersDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            DbSet = context.Set<T>();
        }

        protected DbSet<T> DbSet { get; }

        protected UsersDbContext Context { get; }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await IncludeAll().SingleOrDefaultAsync(i => i.Id == id);
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await DbSet.AddAsync(entity);
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

        public virtual async Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo)
        {
            var query = IncludeAll().OrderBy(s => s.Id).AsQueryable();
            if (pagingInfo.PageSize != 0)
            {
                query = query.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);
            }

            var items = await query.ToListAsync();

            var allItemsCount = await IncludeAll().CountAsync();

            var result = new ReturnPagingInfo<T>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = allItemsCount,
                TotalPages = (int)Math.Ceiling((double)allItemsCount / (pagingInfo.PageSize == 0 ? allItemsCount : pagingInfo.PageSize)),
                Data = items
            };

            return result;
        }

        protected abstract IQueryable<T> IncludeAll();
    }
}
