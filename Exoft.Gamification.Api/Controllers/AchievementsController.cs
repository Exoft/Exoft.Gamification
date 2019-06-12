using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    //AZ: Add metadata foreach action

    [Route("api/achievements")]
    //[Authorize]
    [ApiController]
    public class AchievementsController : GamificationController
    {
        private readonly IAchievementService _achievementService;
        //AZ: better to move uow inside service
        private readonly IUnitOfWork _unitOfWork;

        public AchievementsController
        (
            IAchievementService achievementService,
            IUnitOfWork unitOfWork
        )
        {
            _achievementService = achievementService;
            _unitOfWork = unitOfWork;
        }


        //AZ: paging is missing
        [HttpGet]
        public async Task<IActionResult> GetAchievementsAsync()
        {
            try
            {
                var allItems = await _achievementService.GetAllAsync();
                if(allItems == null)
                {
                    return NotFound();
                }

                return Ok(allItems);
            }
            //AZ: remove all catches.... no need in it
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        //AZ: GetSingleAchievementAsync or GetAchievementByIdAsync would be better
        [HttpGet("{achievementId}")]
        public async Task<IActionResult> GetAchievementAsync(Guid achievementId)
        {
            try
            {
                var item = await _achievementService.GetAchievementByIdAsync(achievementId);
                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpGet("{userId}/achievements")]
        //AZ: better to call GetUserAchievementsAsync
        public async Task<IActionResult> GetAchievementsByUserAsync(Guid userId)
        {
            try
            {
                //AZ: typo here, also its betterto call GetUserAchievementsAsync
                var item = await _achievementService.GetAchievementsDyUserAsync(userId);
                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpGet("{userId}/achievements/{achievementId}")]
        public async Task<IActionResult> GetAchievementByUserAsync(Guid userId, Guid achievementId)
        {
            try
            {
                var models = await _achievementService.GetAchievementsDyUserAsync(userId);
                if (models == null)
                {
                    return NotFound();
                }
                //AZ: please create additional method here.
                var model = models.Select(i => i).Where(i => i.Id == achievementId);
                if(model.Count() == 0)
                {
                    return NotFound();
                }
                    

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAchievementAsync([FromBody] InAchievementModel model)
        {
            try
            {
                //AZ: I do not see any ModelState  check here
                await _achievementService.AddAchievementAsync(model);

                await _unitOfWork.SaveChangesAsync();

                //AZ: response is not correct
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpPut("{achievementId}")]
        public async Task<IActionResult> UpdateAchievementAsync([FromBody] InAchievementModel model, Guid achievementId)
        {
            try
            {
                //AZ: what if achievement does not exist?

                //AZ: I do not see any ModelState  check here
                _achievementService.UpdateAchievement(model, achievementId);

                await _unitOfWork.SaveChangesAsync();

                //AZ: its not correct, update method should return you entity
                var item = await _achievementService.GetAchievementByIdAsync(achievementId);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpDelete("{achievementId}")]
        public async Task<IActionResult> DeleteAchievementAsync(Guid achievementId)
        {
            try
            {
                //AZ: what if achievement does not exist?
                _achievementService.DeleteAchievement(achievementId);

                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }
    }
}