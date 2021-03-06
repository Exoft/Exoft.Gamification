﻿using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateUserModelValidator : BaseValidator<CreateUserModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public CreateUserModelValidator
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

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MinimumLength(8).WithMessage(_stringLocalizer["TooShort"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .Matches("[A-Z]").WithMessage(_stringLocalizer["PasswordUpperCaseLetter"])
                .Matches("[a-z]").WithMessage(_stringLocalizer["PasswordLowerCaseLetter"])
                .Matches("[0-9]").WithMessage(_stringLocalizer["PasswordDigit"]);

            RuleFor(user => user.Roles)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckRolesAsync).WithMessage(_stringLocalizer["WrongRole"]);

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckUserNameAsync).WithMessage(_stringLocalizer["UniqueUserName"]);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.DoesEmailExistsAsync(email, cancellationToken);
            return !exists;
        }

        private async Task<bool> CheckRolesAsync(IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                if (await _roleRepository.GetRoleByNameAsync(role, cancellationToken) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> CheckUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
            return userEntity == null;
        }
    }
}
