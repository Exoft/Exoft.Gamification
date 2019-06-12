using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Repositories
{
    //AZ: i'd recommend you to move repository interfaces into Data.Core project
    // because Repositories it are DAL, services are BLL
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
