using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Resources;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    using Exoft.Gamification.Api.Common.Models.RequestAchievement;

    public class RequestAchievementModelValidator : BaseValidator<CreateRequestAchievementModel>
    {
        private readonly IAchievementService _achievementService;

        public RequestAchievementModelValidator
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
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);

            return achievement != null;
        }
    }
}
