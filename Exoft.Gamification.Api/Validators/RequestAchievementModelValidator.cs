using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class RequestAchievementModelValidator : BaseValidator<RequestAchievementModel>
    {
        public RequestAchievementModelValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer, 
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            RuleFor(model => model.Message)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(400).WithMessage(_stringLocalizer["TooLong"]);
        }
    }
}
