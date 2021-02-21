using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/thanks")]
    [Authorize]
    [ApiController]
    public class ThanksController : GamificationController
    {
        private readonly IThankService _thankService;
        private readonly IValidator<CreateThankModel> _createThankModelValidator;

        public ThanksController
        (
            IThankService thankService,
            IValidator<CreateThankModel> createThankModelValidator
        )
        {
            _thankService = thankService;
            _createThankModelValidator = createThankModelValidator;
        }

        /// <summary>
        /// Create thank for user.
        /// </summary>
        /// <responce code="200">When thank successful created</responce> 
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> SayThanks([FromBody] CreateThankModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _createThankModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            await _thankService.AddAsync(model, UserId, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get thank for current user.
        /// </summary>
        /// <responce code="200">Thank model</responce> 
        [HttpGet]
        public async Task<IActionResult> GetThanks(CancellationToken cancellationToken)
        {
            var thank = await _thankService.GetLastThankAsync(UserId, cancellationToken);

            return Ok(thank);
        }
    }
}