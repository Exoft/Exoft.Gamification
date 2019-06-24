using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : class
    {
        protected IStringLocalizer<ValidatorMessages> _stringLocalizer;

        public BaseValidator(IStringLocalizer<ValidatorMessages> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
    }
}
