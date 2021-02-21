using Exoft.Gamification.Api.Common.Models.Thank;
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
    public class CreateThankModelValidator : BaseValidator<CreateThankModel>
    {
        private readonly IUserRepository _userRepository;

        public CreateThankModelValidator
        (
            IUserRepository userRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _userRepository = userRepository;

            RuleFor(thank => thank.Text)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(100).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(thank => thank.ToUserId)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(DoesUserExistAsync).WithMessage(_stringLocalizer["UserNotFound"]);
        }

        private async Task<bool> DoesUserExistAsync(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == CurrentUserId)
            {
                return false;
            }

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            return user != null;
        }
    }
}
