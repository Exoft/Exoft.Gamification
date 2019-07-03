using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IThankRepository : IRepository<Thank>
    {
        Task<Thank> GetLastThankAsync(Guid toUserId);
    }
}
