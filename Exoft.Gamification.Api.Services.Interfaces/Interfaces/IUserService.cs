using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Interfaces
{
    public interface IUserService
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid Id);
        Task<User> GetUserAsync(Guid Id);
        Task<User> GetUserAsync(string userName);
        Task<IEnumerable<User>> GetUsersAsync();
    }
}
