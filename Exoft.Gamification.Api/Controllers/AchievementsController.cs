using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/achievements")]
    [Authorize]
    [ApiController]
    public class AchievementsController : GamificationController
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }


        /// <summary>
        /// Get paged list of achievements
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of achievements</responce> 
        [HttpGet]
        public async Task<IActionResult> GetAchievementsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var allItems = await _achievementService.GetAllAchievementsAsync(pagingInfo);

            return Ok(allItems);
        }

        /// <summary>
        /// Get some achievement
        /// </summary>
        /// <responce code="200">Return some achievement</responce> 
        /// <responce code="404">When the achievement does not exist</responce> 
        [HttpGet("{achievementId}", Name = "GetAchievement")]
        public async Task<IActionResult> GetAchievementByIdAsync(Guid achievementId)
        {
            var item = await _achievementService.GetAchievementByIdAsync(achievementId);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new achievement
        /// </summary>
        /// <response code="201">Return created achievement</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> AddAchievementAsync([FromForm] CreateAchievementModel model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var achievement = await _achievementService.AddAchievementAsync(model);
            
            return CreatedAtRoute(
                "GetAchievement",
                new { achievementId = achievement.Id },
                achievement);
        }

        /// <summary>
        /// Update achievement
        /// </summary>
        /// <responce code="200">Return the updated achievement</responce> 
        /// <responce code="404">When the achievement does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [HttpPut("{achievementId}")]
        public async Task<IActionResult> UpdateAchievementAsync([FromForm] UpdateAchievementModel model, Guid achievementId)
        {
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);
            if(achievement == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var item = await _achievementService.UpdateAchievementAsync(model, achievementId);

            return Ok(item);
        }

        /// <summary>
        /// Delete achievement by Id
        /// </summary>
        /// <responce code="204">When the achievement successful delete</responce>
        /// <response code="404">When the achievement does not exist</response>
        [HttpDelete("{achievementId}")]
        public async Task<IActionResult> DeleteAchievementAsync(Guid achievementId)
        {
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);
            if (achievement == null)
            {
                return NotFound();
            }
            
            await _achievementService.DeleteAchievementAsync(achievementId);

            return NoContent();
        }
    }
}