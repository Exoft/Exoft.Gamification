using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class ResetPasswordModelValidator : BaseValidator<ResetPasswordModel>
    {
        private readonly ICacheManager<Guid> _cache;

        public ResetPasswordModelValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer, 
            IActionContextAccessor actionContextAccessor,
            ICacheManager<Guid> cache
            
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _cache = cache;

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MinimumLength(8).WithMessage(_stringLocalizer["TooShort"])
                .MaximumLength(32).WithMessage(_stringLocalizer["TooLong"])
                .Matches("[A-Z]").WithMessage(_stringLocalizer["PasswordUpperCaseLetter"])
                .Matches("[a-z]").WithMessage(_stringLocalizer["PasswordLowerCaseLetter"])
                .Matches("[0-9]").WithMessage(_stringLocalizer["PasswordDigit"]);

            RuleFor(model => model.SecretString)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckSecretString).WithMessage(_stringLocalizer["UnknownSecretString"]);
        }

        private async Task<bool> CheckSecretString(string secretString, CancellationToken cancellationToken)
        {
            var userId = await _cache.GetByKeyAsync(secretString, cancellationToken);

            return userId != default(Guid);
        }
    }
}
