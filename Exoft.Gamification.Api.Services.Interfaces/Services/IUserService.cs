using Exoft.Gamification.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(Guid Id);
        Task<UserModel> GetUserAsync(string userName);
        Task<ICollection<UserModel>> GetUsersAsync();
    }
}
