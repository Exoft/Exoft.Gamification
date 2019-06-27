using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IThankRepository : IRepository<Thank>
    {
        Task<Thank> GetThankAsync(Guid toUserId);
    }
}
