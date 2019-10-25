using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void AddRangeAsync(IEnumerable<T> entities);
        void Delete(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);
        Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo);
    }
}
