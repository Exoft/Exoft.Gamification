using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateThankModelValidator : BaseValidator<CreateThankModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        public CreateThankModelValidator
        (
            IUserRepository userRepository,
            IActionContextAccessor actionContextAccessor,
            IStringLocalizer<ValidatorMessages> stringLocalizer
        ) : base(stringLocalizer)
        {
            _userRepository = userRepository;
            _actionContextAccessor = actionContextAccessor;

            RuleFor(thank => thank.Text)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(100).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(thank => thank.ToUserId)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(DoesUserExistAsync).WithMessage(_stringLocalizer["UserNotFound"]);
        }

        private async Task<bool> DoesUserExistAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var currentUserId = Guid.Parse(_actionContextAccessor.ActionContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userId == currentUserId)
            {
                return false;
            }

            return user != null;
        }
    }
}
