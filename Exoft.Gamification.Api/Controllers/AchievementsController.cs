using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/achievements")]
    //[Authorize]
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
        /// <responce code="404">When we haven't any achievements</responce> 
        [HttpGet]
        public async Task<IActionResult> GetAchievementsAsync(int pageNumber, int pageSize)
        {
            PageInfo pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var allItems = await _achievementService.GetPagedAchievement(pageInfo);
            if(allItems == null)
            {
                return NotFound();
            }

            var pagedList = new ReturnPageModel<ReadAchievementModel>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };
            return Ok(pagedList);
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
        /// Get all achievements current user
        /// </summary>
        /// <responce code="200">Return all achievements current user</responce> 
        /// <responce code="404">When the user haven't any achievements</responce> 
        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetUserAchievementsAsync(Guid userId)
        {
            var item = await _achievementService.GetUserAchievementsAsync(userId);
            if (item == null)
            {
                return NotFound();
            }

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
            var models = await _achievementService.GetUserAchievementsAsync(userId);
            if (models == null)
            {
                return NotFound();
            }

            var modelsId = models.Select(i => i.Id);
            if(!modelsId.Contains(achievementId))
            {
                return NotFound();
            }

            var model = _achievementService.GetAchievementByIdAsync(achievementId);

            return Ok(model);
        }

        /// <summary>
        /// Create a new achievement
        /// </summary>
        /// <response code="201">Return created achievement</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> AddAchievementAsync([FromBody] CreateAchievementModel model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            var achievement = await _achievementService.AddAchievementAsync(model);

            await _achievementService.SaveChangesAsync();
            
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
        public async Task<IActionResult> UpdateAchievementAsync([FromBody] UpdateAchievementModel model, Guid achievementId)
        {
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);
            if(achievement == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            var item = _achievementService.UpdateAchievement(model, achievementId);

            await _achievementService.SaveChangesAsync();

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
            
            _achievementService.DeleteAchievementAsync(achievementId);

            await _achievementService.SaveChangesAsync();

            return NoContent();
        }
    }
}