using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        void Delete(T entity);

        void Update(T entity);

        Task<ReturnPagingInfo<T>> GetAllDataAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);
    }
}
