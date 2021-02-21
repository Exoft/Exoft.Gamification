using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Resources;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateRequestAchievementModelValidator : BaseValidator<CreateRequestAchievementModel>
    {
        private readonly IAchievementService _achievementService;

        public CreateRequestAchievementModelValidator
        (
            IAchievementService achievementService,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _achievementService = achievementService;

            RuleFor(model => model.Message)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(400).WithMessage(_stringLocalizer["TooLong"]);
            RuleFor(model => model.AchievementId)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckDoesAchievementExistAsync).WithMessage(_stringLocalizer["AchievementNotFound"]);
        }

        private async Task<bool> CheckDoesAchievementExistAsync(Guid achievementId, CancellationToken cancellationToken)
        {
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId, cancellationToken);

            return achievement != null;
        }
    }
}
