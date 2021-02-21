using Exoft.Gamification.Api.Data.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> DoesEmailExistsAsync(string email, CancellationToken cancellationToken);
        Task<ICollection<string>> GetAdminsEmailsAsync(CancellationToken cancellationToken);
    }
}
