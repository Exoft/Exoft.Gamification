using AutoMapper;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _hasher;

        public UserService
        (
            IUserRepository userRepository,
            IFileRepository fileRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPasswordHasher hasher
        )
        {
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hasher = hasher;
        }

        public async Task<ReadFullUserModel> AddUserAsync(CreateUserModel model)
        {
            var user = _mapper.Map<User>(model);

            var role = await _roleRepository.GetRoleByNameAsync(model.Role);

            var userRole = new UserRoles()
            {
                Role = role,
                User = user
            };

            user.Roles.Add(userRole);

            user.Password = _hasher.GetHash(model.Password);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task DeleteUserAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            _userRepository.Delete(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            
            user.Password = _hasher.GetHash(newPassword);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUserAsync(PagingInfo pagingInfo)
        {
            var page = await _userRepository.GetAllDataAsync(pagingInfo);

            var readUserModel = page.Data.Select(i => _mapper.Map<ReadShortUserModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadShortUserModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readUserModel
            };

            return result;
        }

        public async Task<ReadFullUserModel> GetFullUserByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task<ReadShortUserModel> GetShortUserByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadShortUserModel>(user);
        }

        public async Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Status = model.Status;
            user.Email = model.Email;
            
            if (model.Avatar != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await model.Avatar.CopyToAsync(memory);

                    if(user.AvatarId != null)
                    {
                        await _fileRepository.Delete(user.AvatarId.Value);
                    }   

                    var file = new File()
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Avatar.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    user.AvatarId = file.Id;
                }
            }
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task<ReadFullUserModel> UpdateUserAsync(UpdateFullUserModel model, Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Status = model.Status;
            user.Email = model.Email;
            
        
            var role = await _roleRepository.GetRoleByNameAsync(model.Role);

            var userRole = new UserRoles()
            {
                Role = role,
                User = user
            };

            user.Roles.Clear();
            user.Roles.Add(userRole);


            if (model.Avatar != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await model.Avatar.CopyToAsync(memory);

                    if (user.AvatarId != null)
                    {
                        await _fileRepository.Delete(user.AvatarId.Value);
                    }

                    var file = new File()
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Avatar.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    user.AvatarId = file.Id;
                }
            }
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }
    }
}
