﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;

using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
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
            IFileService fileService,
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
            _fileService = fileService;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hasher = hasher;
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
            _userAchievementRepository = userAchievementRepository;
        }

        public async Task<ReadFullUserModel> AddUserAsync(CreateUserModel model, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(model);

            await SetRolesForUserModel(model.Roles, user, cancellationToken);

            user.AvatarId = await _fileService.AddOrUpdateFileByIdAsync(model.Avatar, user.AvatarId, cancellationToken);

            user.Password = _hasher.GetHash(model.Password);

            await _userRepository.AddAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await SendEmail(model, cancellationToken);

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            _userRepository.Delete(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            user.Password = _hasher.GetHash(newPassword);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUsersWithShortInfoAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var page = await _userRepository.GetAllDataAsync(pagingInfo, cancellationToken);

            var readUserModels = page.Data.Select(i => _mapper.Map<ReadShortUserModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadShortUserModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readUserModels
            };

            return result;
        }

        public async Task<ReturnPagingInfo<ReadFullUserModel>> GetAllUsersWithFullInfoAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var page = await _userRepository.GetAllDataAsync(pagingInfo, cancellationToken);

            var readUserModels = page.Data.Select(i => _mapper.Map<ReadFullUserModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadFullUserModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readUserModels
            };

            return result;
        }

        public async Task<ReadFullUserModel> GetFullUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            var fullUserModel = _mapper.Map<ReadFullUserModel>(user);
            fullUserModel.BadgesCount = await _userAchievementRepository.GetCountAchievementsByUserAsync(user.Id, cancellationToken);

            return fullUserModel;
        }

        public async Task<ReadShortUserModel> GetShortUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<ReadShortUserModel>(user);
        }

        public async Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Status = model.Status;
            user.Email = model.Email;

            if (model is UpdateFullUserModel updateFullUserModel)
            {
                user.Roles.Clear();
                await SetRolesForUserModel(updateFullUserModel.Roles, user, cancellationToken);
            }

            user.AvatarId = await _fileService.AddOrUpdateFileByIdAsync(model.Avatar, user.AvatarId, cancellationToken);
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReadFullUserModel>(user);
        }
        
        private async Task SetRolesForUserModel(IEnumerable<string> roles, User user, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                var roleFromDb = await _roleRepository.GetRoleByNameAsync(role, cancellationToken);

                var userRole = new UserRoles
                {
                    Role = roleFromDb,
                    User = user
                };

                user.Roles.Add(userRole);
            }
        }

        private async Task SendEmail(CreateUserModel model, CancellationToken cancellationToken)
        {
            var forgotPasswordPage = _stringLocalizer["RegisterPage"].ToString();

            var pageWithParams = forgotPasswordPage.Replace("{FirstName}", model.FirstName).Replace("{Password}", model.Password);

            await _emailService.SendEmailAsync("Welcome to Exoft", pageWithParams, model.Email, cancellationToken);
        }
    }
}
