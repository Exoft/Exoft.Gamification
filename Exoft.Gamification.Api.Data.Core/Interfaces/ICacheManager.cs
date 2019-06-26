using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface ICacheManager<T> 
    {
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByKeyAsync(string key);
    }
}
