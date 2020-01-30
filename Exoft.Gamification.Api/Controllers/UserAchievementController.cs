using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserAchievementController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IAchievementService _achievementService;
        private readonly IUserAchievementService _userAchievementService;

        public UserAchievementController
        (
            IUserService userService,
            IAchievementService achievementService,
            IUserAchievementService userAchievementsService
        )
        {
            _userService = userService;
            _achievementService = achievementService;
            _userAchievementService = userAchievementsService;
        }


        /// <summary>
        /// Add or remove achievements into user
        /// </summary>
        /// <responce code="200">Return Ok</responce> 
        /// <responce code="404">When any achievement or user does not exist</responce> 
        [Authorize(Policy = "IsAdmin")]
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
        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetUserAchievementsAsync(Guid userId, [FromQuery] PagingInfo pagingInfo)
        {
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