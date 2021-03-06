﻿using System;
using System.Threading;
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
        private readonly IRequestAchievementService _requestAchievementService;
        private readonly IValidator<CreateRequestAchievementModel> _createRequestAchievementModelValidator;

        public RequestAchievementController
        (
            IRequestAchievementService requestAchievementService,
            IValidator<CreateRequestAchievementModel> createRequestAchievementModelValidator
        )
        {
            _requestAchievementService = requestAchievementService;
            _createRequestAchievementModelValidator = createRequestAchievementModelValidator;
        }

        /// <summary>
        /// Create new request achievement
        /// </summary>
        /// <response code="200">When request success sent and added</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> AddRequestAsync([FromBody] CreateRequestAchievementModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _createRequestAchievementModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _requestAchievementService.AddAsync(model, UserId, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Return all achievement requests 
        /// </summary>
        /// <response code="200">Return all requests</response>
        /// <response code="403">When user don't have permissions to this action</response>
        [HttpGet]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> GetAllAchievementRequests(CancellationToken cancellationToken)
        {
            return Ok(await _requestAchievementService.GetAllAsync(cancellationToken));
        }

        /// <summary>
        /// Delete request 
        /// </summary>
        /// <response code="200">Decline achievement request with current Id</response>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When request with current Id is not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> DeclineRequest(Guid id, CancellationToken cancellationToken)
        {
            var achievementRequest = await _requestAchievementService.GetByIdAsync(id, cancellationToken);
            if (achievementRequest == null)
            {
                return NotFound();
            }

            await _requestAchievementService.DeleteAsync(achievementRequest, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Approve request  
        /// </summary>
        /// <response code="200">When request is approved</response>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When request with current Id is not found</response>
        [HttpPost("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> ApproveRequest(Guid id, CancellationToken cancellationToken)
        {
            var achievementRequest = await _requestAchievementService.GetByIdAsync(id, cancellationToken);
            if (achievementRequest == null)
            {
                return NotFound();
            }

            await _requestAchievementService.ApproveAchievementRequestAsync(id, cancellationToken);

            return Ok();
        }
    }
}