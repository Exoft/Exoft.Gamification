using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/request-achievement")]
    [Authorize]
    [ApiController]
    public class RequestAchievementController : GamificationController
    {
        private readonly IAchievementService _achievementService;
        private readonly IRequestAchievementService _requestAchievementService;
        private readonly IValidator<CreateRequestAchievementModel> _requestAchievementModelValidator;

        public RequestAchievementController
        (
            IAchievementService achievementService,
            IRequestAchievementService requestAchievementService,
            IValidator<CreateRequestAchievementModel> requestAchievementModelValidator
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
        public async Task<IActionResult> AddRequestAsync([FromBody] CreateRequestAchievementModel model)
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
        public async Task<IActionResult> DeclineRequest(Guid id)
        {
            var achievementRequest = await _requestAchievementService.GetByIdAsync(id);
            if (achievementRequest == null)
            {
                return NotFound();
            }

            await _requestAchievementService.DeleteAsync(achievementRequest);
            return Ok();
        }


        /// <summary>
        /// Approves Request  
        /// </summary>
        [HttpPost("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            await _requestAchievementService.ApproveAchievementRequestAsync(id);

            return Ok();
        }
    }
}