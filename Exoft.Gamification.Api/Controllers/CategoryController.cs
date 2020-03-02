using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/categories")]
    [Authorize]
    [ApiController]
    public class CategoryController : GamificationController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController
        (
            ICategoryService categoryService
        )
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get paged list of categories
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of categories</responce> 
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] PagingInfo pagingInfo)
        {
            var list = await _categoryService.GetAllCategoryAsync(pagingInfo);

            return Ok(list);
        }
    }
}
