using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Security.Claims;

namespace Exoft.Gamification.Api.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : class
    {
        protected readonly IStringLocalizer<ValidatorMessages> _stringLocalizer;
        protected readonly IActionContextAccessor _actionContextAccessor;

        public BaseValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        )
        {
            _stringLocalizer = stringLocalizer;
            _actionContextAccessor = actionContextAccessor;
        }

        protected Guid CurrentUserId => Guid.Parse(_actionContextAccessor.ActionContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
