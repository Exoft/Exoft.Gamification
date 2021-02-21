using Exoft.Gamification.Api.Common.Models.Achievement;
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
    public class UpdateAchievementModelValidator : BaseValidator<UpdateAchievementModel>
    {
        private readonly IAchievementRepository _achievementRepository;

        public UpdateAchievementModelValidator
        (
            IAchievementRepository achievementRepository,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _achievementRepository = achievementRepository;

            RuleFor(achievement => achievement.Name)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(70).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckNameAsync).WithMessage(_stringLocalizer["UniqueName"]);

            RuleFor(achievement => achievement.Description)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(250);

            RuleFor(achievement => achievement.XP)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .GreaterThanOrEqualTo(0).WithMessage(_stringLocalizer["SmallValue"]);
        }

        private async Task<bool> CheckNameAsync(string name, CancellationToken cancellationToken)
        {
            var achievementId = GetAchievementId();
            var achievementEntity = await _achievementRepository.GetByIdAsync(achievementId, cancellationToken);

            if(achievementEntity.Name == name)
            {
                return true;
            }

            var achievement = await _achievementRepository.GetAchievementByNameAsync(name, cancellationToken);
            if(achievement == null)
            {
                return true;
            }

            return achievementEntity.Id == achievement.Id;
        }

        private Guid GetAchievementId()
        {
            var achievementIdString = _actionContextAccessor.ActionContext.RouteData.Values["achievementId"].ToString();

            return Guid.Parse(achievementIdString);
        }
    }
}
