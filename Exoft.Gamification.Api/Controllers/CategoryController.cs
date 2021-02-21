using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/categories")]
    [Authorize]
    [ApiController]
    public class CategoryController : GamificationController
    {
        private readonly IValidator<CreateCategoryModel> _createCategoryModelValidator;
        private readonly IValidator<UpdateCategoryModel> _updateCategoryModelValidator;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;
        private readonly ICategoryService _categoryService;

        public CategoryController
        (
            IValidator<CreateCategoryModel> createCategoryModelValidator,
            IValidator<UpdateCategoryModel> updateCategoryModelValidator,
            IValidator<PagingInfo> pagingInfoValidator,
            ICategoryService categoryService
        )
        {
            _createCategoryModelValidator = createCategoryModelValidator;
            _updateCategoryModelValidator = updateCategoryModelValidator;
            _pagingInfoValidator = pagingInfoValidator;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get paged list of categories
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of categories</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var list = await _categoryService.GetAllCategoryAsync(pagingInfo, cancellationToken);

            return Ok(list);
        }

        /// <summary>
        /// Get category by Id
        /// </summary>
        /// <responce code="200">Return some category</responce> 
        /// <responce code="404">When category does not exist</responce> 
        [HttpGet("{categoryId}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var item = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <responce code="201">Return created category</responce> 
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync([FromForm] CreateCategoryModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _createCategoryModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var category = await _categoryService.AddCategoryAsync(model, cancellationToken);

            return CreatedAtRoute(
                "GetCategory",
                new { categoryId = category.Id },
                category);
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <responce code="200">Return the updated category</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <responce code="404">When the order does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromForm] UpdateCategoryModel model, Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateCategoryModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _categoryService.UpdateCategoryAsync(model, categoryId, cancellationToken);

            return Ok(item);
        }

        /// <summary>
        /// Delete category by Id
        /// </summary>
        /// <responce code="204">When the category successful delete</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When the category does not exist</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(categoryId, cancellationToken);

            return NoContent();
        }
    }
}
