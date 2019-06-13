using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<ReturnPagingInfo<T>> GetPagingDataAsync(PagingInfo pagingInfo);
    }
}
