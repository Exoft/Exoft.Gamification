using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Resources;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class PagingInfoValidator : BaseValidator<PagingInfo>
    {
        public PagingInfoValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            RuleFor(info => info.CurrentPage)
                .GreaterThanOrEqualTo(1).WithMessage(_stringLocalizer["SmallValue"]);

            RuleFor(info => info.PageSize)
                .GreaterThanOrEqualTo(0).WithMessage(_stringLocalizer["SmallValue"]);
        }
    }
}
