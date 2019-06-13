using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public async Task<ReturnPagingInfo<T>> GetPagingDataAsync(PagingInfo pagingInfo)
        {
            var items = await IncludeAll()
                .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToListAsync();

            var result = new ReturnPagingInfo<T>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = IncludeAll().Count(),
                TotalPages = (int)Math.Ceiling((double)IncludeAll().Count() / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }

        protected abstract IQueryable<T> IncludeAll();
    }
}
