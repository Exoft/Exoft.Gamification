using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateOrderModelValidator : BaseValidator<CreateOrderModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateOrderModelValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor,
            ICategoryRepository categoryRepository
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _categoryRepository = categoryRepository;

            RuleFor(order => order.Title)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(70).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(order => order.Description)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(250).WithMessage(_stringLocalizer["TooLong"]);

            RuleFor(order => order.Price)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .GreaterThanOrEqualTo(0).WithMessage(_stringLocalizer["SmallValue"]);

            RuleFor(order => order.Icon)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"]);

            RuleFor(order => order.CategoryIds)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MustAsync(CheckCategoriesAsync).WithMessage(_stringLocalizer["CategoryNotFound"]);
        }

        private async Task<bool> CheckCategoriesAsync(IEnumerable<Guid> categoryIds, CancellationToken cancellationToken)
        {
            foreach (var categoryId in categoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);

                if (category == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
