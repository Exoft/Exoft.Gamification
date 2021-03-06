﻿using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class UpdateUserModelValidator : BaseValidator<UpdateUserModel>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserModelValidator
        (
            IUserRepository userRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _userRepository = userRepository;

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

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckUserNameAsync).WithMessage(_stringLocalizer["UniqueUserName"]);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(CurrentUserId, cancellationToken);

            if (user.Email == email)
            {
                return true;
            }

            var exists = await _userRepository.DoesEmailExistsAsync(email, cancellationToken);
            return !exists;
        }

        private async Task<bool> CheckUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(CurrentUserId, cancellationToken);

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
    }
}
