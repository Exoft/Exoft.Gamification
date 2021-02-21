using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class ChangePasswordModelValidator : BaseValidator<ChangePasswordModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public ChangePasswordModelValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor,
            IUserRepository userRepository,
            IPasswordHasher hasher
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _userRepository = userRepository;
            _hasher = hasher;

            RuleFor(model => model.OldPassword)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckPasswordAsync).WithMessage(_stringLocalizer["WrongPassword"]);

            RuleFor(model => model.NewPassword)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MinimumLength(8).WithMessage(_stringLocalizer["TooShort"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .Matches("[A-Z]").WithMessage(_stringLocalizer["PasswordUpperCaseLetter"])
                .Matches("[a-z]").WithMessage(_stringLocalizer["PasswordLowerCaseLetter"])
                .Matches("[0-9]").WithMessage(_stringLocalizer["PasswordDigit"])
                .MustAsync(ComparePasswordAsync).WithMessage(_stringLocalizer["OldPassword"]);

            RuleFor(model => model.ConfirmNewPassword)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .Equal(model => model.NewPassword).WithMessage(_stringLocalizer["NonEqualsPassword"]);
        }

        private async Task<bool> CheckPasswordAsync(string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(CurrentUserId, cancellationToken);

            return user.Password == _hasher.GetHash(password);
        }

        private async Task<bool> ComparePasswordAsync(string newPassword, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(CurrentUserId, cancellationToken);

            return user.Password != _hasher.GetHash(newPassword);
        }
    }
}
