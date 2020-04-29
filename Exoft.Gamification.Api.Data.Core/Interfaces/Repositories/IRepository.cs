using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id);

        Task AddAsync(T entity);

        void Delete(T entity);

        void Update(T entity);

        Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo);
    }
}
