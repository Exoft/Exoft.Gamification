using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Resources;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Validators
{
    public class UpdateCategoryModelValidator : BaseValidator<UpdateCategoryModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryModelValidator
        (
            IStringLocalizer<ValidatorMessages> stringLocalizer,
            IActionContextAccessor actionContextAccessor,
            ICategoryRepository categoryRepository
        ) : base(stringLocalizer, actionContextAccessor)
        {
            _categoryRepository = categoryRepository;

            RuleFor(category => category.Name)
                .NotEmpty().WithMessage(_stringLocalizer["EmptyField"])
                .MaximumLength(30).WithMessage(_stringLocalizer["TooLong"])
                .MustAsync(CheckNameAsync).WithMessage(_stringLocalizer["UniqueName"]);
        }

        private async Task<bool> CheckNameAsync(string name, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(name, cancellationToken);

            return category == null;
        }
    }
}
