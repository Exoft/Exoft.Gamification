using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class RequestResetPasswordModelValidator : BaseValidator<RequestResetPasswordModel>
    {
        private readonly IUserRepository _userRepository;

        public RequestResetPasswordModelValidator
        (
            IUserRepository userRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer, 
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _userRepository = userRepository;

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage(_stringLocalizer["EmtpyField"])
                .EmailAddress().WithMessage(_stringLocalizer["WrongEmail"])
                .MustAsync(CheckEmailAsync).WithMessage(_stringLocalizer["NotFound"]);

            RuleFor(model => model.ResetPasswordPageLink)
                .NotEmpty().WithMessage(_stringLocalizer["EmtpyField"]);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            bool exists = await _userRepository.DoesEmailExistsAsync(email);
            return exists;
        }
    }
}
