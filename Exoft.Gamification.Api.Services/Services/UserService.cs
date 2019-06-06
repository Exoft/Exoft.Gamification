using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Services
{
    public class UserService : IUserService
    {
        private UserRepository userRepository;

        public UserService(UsersDbContext context)
        {
            userRepository = new UserRepository(context);
        }

        public async Task AddAsync(User user)
        {
            await userRepository.AddAsync(user);
        }

        public async Task DeleteAsync(Guid Id)
        {
            await userRepository.DeleteAsync(Id);
        }

        public async Task<User> GetUserAsync(Guid Id)
        {
            return await userRepository.GetUserAsync(Id);
        }

        public async Task<User> GetUserAsync(string userName)
        {
            return await userRepository.GetUserAsync(userName);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await userRepository.GetUsersAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await userRepository.UpdateAsync(user);
        }
    }
}
