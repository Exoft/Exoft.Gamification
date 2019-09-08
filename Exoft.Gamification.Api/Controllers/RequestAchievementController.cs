using Exoft.Gamification.Api.Common.Models;
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
    [Route("api/request-achievement")]
    [Authorize]
    [ApiController]
    public class RequestAchievementController : GamificationController
    {
        private readonly IAchievementService _achievementService;
        private readonly IRequestAchievementService _requestAchievementService;
        private readonly IValidator<RequestAchievementModel> _requestAchievementModelValidator;

        public RequestAchievementController
        (
            IAchievementService achievementService,
            IRequestAchievementService requestAchievementService,
            IValidator<RequestAchievementModel> requestAchievementModelValidator
        )
        {
            _achievementService = achievementService;
            _requestAchievementService = requestAchievementService;
            _requestAchievementModelValidator = requestAchievementModelValidator;
        }

        /// <summary>
        /// Create new request achievement
        /// </summary>
        /// <response code="200">When request success sended and added</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> AddRequestAsync([FromBody] RequestAchievementModel model)
        {
            var resultValidation = await _requestAchievementModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _requestAchievementService.AddAsync(model, UserId);

            return Ok();
        }

        /// <summary>
        /// Returns all achievement requests 
        /// </summary>
        [HttpGet]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> GetAllAchievementRequests()
        {
            return Ok(await _requestAchievementService.GetAllAsync());
        }

        /// <summary>
        /// Deletes Request 
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> DeclineRequest([FromRoute] Guid id)
        {
            try
            {
                var achievementRequest = await _requestAchievementService.GetByIdAsync(id);
                if (achievementRequest == null)
                {
                    return NotFound();
                }
                await _requestAchievementService.DeleteAsync(achievementRequest);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Approves Request  
        /// </summary>
        [HttpPost("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> ApproveRequest([FromRoute] Guid id)
        {
            try
            {
                await _requestAchievementService.ApproveAchievementRequestAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}