using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class PushRequestModelValidator : BaseValidator<PushRequestModel>
    {
        public PushRequestModelValidator(
            IStringLocalizer<ValidatorMessages> stringLocalizer, 
            IActionContextAccessor actionContextAccessor) 
            : base(stringLocalizer, actionContextAccessor)
        {
            RuleFor(request => request)
            .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);
            RuleFor(request => request.PushMessage)
            .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);
            RuleFor(request => request.PushMessage.NotificationContent)
            .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);
            RuleFor(request => request.PushMessage.NotificationContent.Body)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);
            RuleFor(request => request.PushMessage.NotificationContent.Name)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);
            RuleFor(request => request.PushMessage.NotificationContent.Title)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);

        }
    }
}
