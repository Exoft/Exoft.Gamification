using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserAchievementController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IUserAchievementService _userAchievementService;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public UserAchievementController
        (
            IUserService userService,
            IUserAchievementService userAchievementsService,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _userService = userService;
            _userAchievementService = userAchievementsService;
            _pagingInfoValidator = pagingInfoValidator;
        }

        /// <summary>
        /// Add or remove achievements into user
        /// </summary>
        /// <responce code="200">Return Ok</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <responce code="404">When any achievement or user does not exist</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost("{userId}/achievements/")]
        public async Task<IActionResult> UpdateUserAchievements([FromBody] AssignAchievementsToUserModel model, Guid userId)
        {
            var user = await _userService.GetShortUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userAchievementService.ChangeUserAchievementsAsync(model, userId);

            return Ok();
        }

        /// <summary>
        /// Get paged list of achievements current user
        /// </summary>
        /// <responce code="200">Return all achievements current user</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetUserAchievementsAsync(Guid userId, [FromQuery] PagingInfo pagingInfo)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _userAchievementService.GetAllAchievementsByUserAsync(pagingInfo, userId);

            return Ok(item);
        }

        /// <summary>
        /// Get some achievement current user
        /// </summary>
        /// <responce code="200">Return some achievement current user</responce> 
        /// <responce code="404">When the achievement does not exist</responce> 
        [HttpGet("{userId}/achievements/{achievementId}")]
        public async Task<IActionResult> GetAchievementByUserAsync([FromQuery] Guid userAchievementId)
        {
            var model = await _userAchievementService.GetUserAchievementByIdAsync(userAchievementId);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}