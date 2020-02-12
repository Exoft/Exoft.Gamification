using AutoMapper;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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
        private readonly IStringLocalizer<HtmlPages> _stringLocalizer;
        private readonly IEmailService _emailService;
        private readonly IUserAchievementRepository _userAchievementRepository;

        public UserService
        (
            IUserRepository userRepository,
            IFileRepository fileRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPasswordHasher hasher,
            IStringLocalizer<HtmlPages> stringLocalizer,
            IEmailService emailService,
            IUserAchievementRepository userAchievementRepository
        )
        {
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hasher = hasher;
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
            _userAchievementRepository = userAchievementRepository;
        }

        public async Task<ReadFullUserModel> AddUserAsync(CreateUserModel model)
        {
            var user = _mapper.Map<User>(model);

            await SetRolesForUserModel(model.Roles, user);

            await SetAvatarForUserModel(model.Avatar, user);

            user.Password = _hasher.GetHash(model.Password);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            await SendEmail(model);

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

        public async Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUsersWithShortInfoAsync(PagingInfo pagingInfo)
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

        public async Task<ReturnPagingInfo<ReadFullUserModel>> GetAllUsersWithFullInfoAsync(PagingInfo pagingInfo)
        {
            var page = await _userRepository.GetAllDataAsync(pagingInfo);

            var readUserModel = page.Data.Select(i => _mapper.Map<ReadFullUserModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadFullUserModel>()
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

            var fullUserModel = _mapper.Map<ReadFullUserModel>(user);
            fullUserModel.BadgesCount = await _userAchievementRepository.GetCountAchievementsByUserAsync(user.Id);

            return fullUserModel;
        }

        public async Task<ReadShortUserModel> GetShortUserByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadShortUserModel>(user);
        }

        public async Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Status = model.Status;
            user.Email = model.Email;

            if (model is UpdateFullUserModel)
            {
                user.Roles.Clear();
                await SetRolesForUserModel((model as UpdateFullUserModel).Roles, user);
            }

            await SetAvatarForUserModel(model.Avatar, user);
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }

        private async Task SetAvatarForUserModel(IFormFile image, User user)
        {
            if (image != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await image.CopyToAsync(memory);

                    if (user.AvatarId != null)
                    {
                        await _fileRepository.Delete(user.AvatarId.Value);
                    }

                    var file = new File()
                    {
                        Data = memory.ToArray(),
                        ContentType = image.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    user.AvatarId = file.Id;
                }
            }
        }

        private async Task SetRolesForUserModel(IEnumerable<string> roles, User user)
        {
            foreach (var role in roles)
            {
                var roleFromDB = await _roleRepository.GetRoleByNameAsync(role);

                var userRole = new UserRoles()
                {
                    Role = roleFromDB,
                    User = user
                };

                user.Roles.Add(userRole);
            }
        }

        private async Task SendEmail(CreateUserModel model)
        {
            var forgotPasswordPage = _stringLocalizer["RegisterPage"].ToString();

            var pageWithParams = forgotPasswordPage.Replace("{FirstName}", model.FirstName).Replace("{Password}", model.Password);

            await _emailService.SendEmailAsync("Welcome to Exoft", pageWithParams, model.Email);
        }
    }
}
