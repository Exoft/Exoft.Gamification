using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}