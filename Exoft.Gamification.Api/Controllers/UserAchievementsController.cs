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
    public class UserAchievementsController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IAchievementService _achievementService;
        private readonly IUserAchievementsService _userAchievementsService;

        public UserAchievementsController
        (
            IUserService userService,
            IAchievementService achievementService,
            IUserAchievementsService userAchievementsService
        )
        {
            _userService = userService;
            _achievementService = achievementService;
            _userAchievementsService = userAchievementsService;
        }

        /// <summary>
        /// Add achievement to user
        /// </summary>
        /// <responce code="200">Return Ok</responce> 
        /// <responce code="404">When the achievement or user does not exist</responce> 
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost("{userId}/achievements/")]
        public async Task<IActionResult> AddAchievementToUser([FromQuery] Guid achievementId, Guid userId)
        {
            var user = await _userService.GetFullUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);
            if(achievement == null)
            {
                return NotFound();
            }

            await _userAchievementsService.AddAsync(userId, achievementId);
            return Ok();
        }

        /// <summary>
        /// Remove some achievement into user
        /// </summary>
        /// <responce code="204">When the achievement successful delete</responce>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpDelete("{userId}/achievements/{achievementId}")]
        public async Task<IActionResult> DeleteAchievementIntoUser(Guid userAchievementsId)
        {
            await _userAchievementsService.DeleteAsync(userAchievementsId);

            return NoContent();
        }

        /// <summary>
        /// Get paged list of achievements current user
        /// </summary>
        /// <responce code="200">Return all achievements current user</responce> 
        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetUserAchievementsAsync(Guid userId, [FromQuery] PagingInfo pagingInfo)
        {
            var item = await _userAchievementsService.GetAllAchievementsByUserAsync(pagingInfo, userId);

            return Ok(item);
        }

        /// <summary>
        /// Get some achievements current user
        /// </summary>
        /// <responce code="200">Return some achievement current user</responce> 
        /// <responce code="404">When the achievement does not exist</responce> 
        [HttpGet("{userId}/achievements/{achievementId}")]
        public async Task<IActionResult> GetAchievementByUserAsync(Guid userId, Guid achievementId)
        {
            var model = await _userAchievementsService.GetSingleUserAchievementAsync(userId, achievementId);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}