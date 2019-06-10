using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Services
{
    public class UserService : IUserService
    {
        private UserRepository userRepository;
        private IMapper mapper;

        public UserService(UsersDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = new UserRepository(context);
        }

        public async Task<UserModel> GetUserAsync(Guid Id)
        {
            var userEntity = await userRepository.GetUserAsync(Id);
            var userModel = mapper.Map<UserModel>(userEntity);

            return userModel;
        }

        public async Task<UserModel> GetUserAsync(string userName)
        {
            var userEntity = await userRepository.GetUserAsync(userName);
            var userModel = mapper.Map<UserModel>(userEntity);

            return userModel;
        }

        public async Task<ICollection<UserModel>> GetUsersAsync()
        {
            var userEntities = await userRepository.GetUsersAsync();

            var userModels = new List<UserModel>();
            foreach (var item in userEntities)
            {
                userModels.Add(mapper.Map<UserModel>(item));
            }

            return userModels;
        }
    }
}
