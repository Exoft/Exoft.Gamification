using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        public Repository(UsersDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            DbSet = context.Set<T>();
        }

        protected DbSet<T> DbSet { get; }
        protected UsersDbContext Context { get; }

        public T GetById(Guid id)
        {
            return IncludeAll().SingleOrDefault(i => i.Id == id);
        }

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
            var query = IncludeAll().OrderBy(s => s.Id);
            if (pagingInfo.PageSize != 0)
            {
                query.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);
            }

            var items = await query.ToListAsync();

            int allItemsCount = await IncludeAll().CountAsync();

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

        public void AddRangeAsync(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            Context.Set<T>().UpdateRange(entities);
        }
    }
}
