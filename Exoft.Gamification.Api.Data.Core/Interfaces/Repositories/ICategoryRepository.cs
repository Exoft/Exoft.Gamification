using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken);
    }
}
