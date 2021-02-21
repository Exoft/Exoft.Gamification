using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
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
                .NotEmpty().WithMessage(_stringLocalizer["EmtpyField"])
                .Must(CheckUri).WithMessage(_stringLocalizer["InvalidUri"]);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.DoesEmailExistsAsync(email, cancellationToken);
            return exists;
        }

        private static bool CheckUri(Uri uri)
        {
            if (uri == null || string.IsNullOrEmpty(uri.ToString()))
            {
                return false;
            }

            return Uri.TryCreate(uri.ToString(), UriKind.Absolute, out var outUri)
                && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
