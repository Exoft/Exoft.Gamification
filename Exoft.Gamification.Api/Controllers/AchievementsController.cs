using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        private readonly IValidator<CreateAchievementModel> _createAchievementModelValidator;
        private readonly IValidator<UpdateAchievementModel> _updateAchievementModelValidator;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public AchievementsController
        (
            IAchievementService achievementService,
            IValidator<CreateAchievementModel> createAchievementModelValidator,
            IValidator<UpdateAchievementModel> updateAchievementModelValidator,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _achievementService = achievementService;
            _createAchievementModelValidator = createAchievementModelValidator;
            _updateAchievementModelValidator = updateAchievementModelValidator;
            _pagingInfoValidator = pagingInfoValidator;
        }


        /// <summary>
        /// Get paged list of achievements
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of achievements</responce> 
        [HttpGet]
        public async Task<IActionResult> GetAchievementsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

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
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddAchievementAsync([FromForm] CreateAchievementModel model)
        {
            var resultValidation = await _createAchievementModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

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
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPut("{achievementId}")]
        public async Task<IActionResult> UpdateAchievementAsync([FromForm] UpdateAchievementModel model, Guid achievementId)
        {
            var achievement = await _achievementService.GetAchievementByIdAsync(achievementId);
            if(achievement == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateAchievementModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

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
        [Authorize(Roles = GamificationRole.Admin)]
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