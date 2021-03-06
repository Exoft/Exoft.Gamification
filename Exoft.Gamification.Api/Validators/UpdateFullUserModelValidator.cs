﻿using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class UpdateFullUserModelValidator : BaseValidator<UpdateFullUserModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateFullUserModelValidator
        (
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(250).WithMessage(_stringLocalizer["TooLong"])
                .EmailAddress().WithMessage(_stringLocalizer["WrongEmail"])
                .MustAsync(CheckEmailAsync).WithMessage(_stringLocalizer["UniqueEmail"]);

            RuleFor(user => user.Status)
                .MaximumLength(250).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(user => user.Roles)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckRoleAsync).WithMessage(_stringLocalizer["WrongRole"]);

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckUserNameAsync).WithMessage(_stringLocalizer["UniqueUserName"]);
        }


        private async Task<bool> CheckRoleAsync(ICollection<string> roles, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                var roleEntity = await _roleRepository.GetRoleByNameAsync(role, cancellationToken);
                if (roleEntity == null)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (_actionContextAccessor.ActionContext.RouteData.Values.TryGetValue("userId", out var userIdObject))
            {
                if (!Guid.TryParse(userIdObject as string, out var userId))
                {
                    return false;
                }

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user.Email == email)
                {
                    return true;
                }

                bool exists = await _userRepository.DoesEmailExistsAsync(email, cancellationToken);
                return !exists;
            }
            else
            {
                return false;
            }
        }


        private async Task<bool> CheckUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            if (_actionContextAccessor.ActionContext.RouteData.Values.TryGetValue("userId", out var userIdObject))
            {
                if (!Guid.TryParse(userIdObject as string, out var userId))
                {
                    return false;
                }

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user.UserName == userName)
                {
                    return true;
                }

                var userEntity = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
                if (userEntity == null)
                {
                    return true;
                }

                return userEntity.Id == user.Id;
            }
            else
            {
                return false;
            }
        }
    }
}
