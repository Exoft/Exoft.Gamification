using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo);
    }
}
