using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdatePassword(Guid userId, string newPassword);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByEmailAsync(string email);
        Task<bool> DoesEmailExistsAsync(string email);
    }
}
