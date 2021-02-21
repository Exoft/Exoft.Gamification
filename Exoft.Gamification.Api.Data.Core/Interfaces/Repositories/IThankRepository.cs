using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces.Repositories
{
    public interface IThankRepository : IRepository<Thank>
    {
        Task<Thank> GetLastThankAsync(Guid toUserId, CancellationToken cancellationToken);
    }
}
