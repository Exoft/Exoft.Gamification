using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> GetUserAsync(Guid Id);
        Task<User> GetUserAsync(string userName);
    }
}
