using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestOrder;
using Exoft.Gamification.Api.Resources;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateRequestOrderModelValidator : BaseValidator<CreateRequestOrderModel>
    {
        private readonly IOrderService _orderService;

        public CreateRequestOrderModelValidator
        (
            IOrderService orderService,
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _orderService = orderService;

            RuleFor(model => model.Message)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(400).WithMessage(_stringLocalizer["TooLong"]);
            RuleFor(model => model.OrderId)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckDoesOrderExistAsync).WithMessage(_stringLocalizer["OrderNotFound"]);
        }

        private async Task<bool> CheckDoesOrderExistAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

            return order != null;
        }
    }
}
