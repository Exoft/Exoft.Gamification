using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        Task<ICollection<Achievement>> GetAchievementsByUserAsync(Guid Id);
    }
}
