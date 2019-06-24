using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStringLocalizer<ValidatorMessages> _stringLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateUserModelValidator
        (
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _stringLocalizer = stringLocalizer;
            _httpContextAccessor = httpContextAccessor;

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

            RuleFor(user => user.Role)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckRoleAsync).WithMessage(_stringLocalizer["WrongRole"]);

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckUserNameAsync).WithMessage(_stringLocalizer["UniqueUserName"]);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            if(user.Email == email)
            {
                return true;
            }

            bool exists = await _userRepository.IsEmailExistsAsync(email);
            return !exists;
        }

        private async Task<bool> CheckRoleAsync(string role, CancellationToken cancellationToken)
        {
            var roleEntity = await _roleRepository.GetRoleByNameAsync(role);
            return roleEntity != null;
        }

        private async Task<bool> CheckUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            if (user.UserName == userName)
            {
                return true;
            }

            var userEntity = await _userRepository.GetByUserNameAsync(userName);
            if(userEntity == null)
            {
                return true;
            }

            return userEntity.Id == user.Id;
        }

        private Guid GetUserId()
        {
            var path = _httpContextAccessor.HttpContext.Request.Path.Value.ToString();
            var count = path.LastIndexOf('/') + 1;
            var userIdString = path.Substring(count);

            return Guid.Parse(userIdString);
        }
    }
}
