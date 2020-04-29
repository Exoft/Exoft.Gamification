using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestOrder;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/request-order")]
    [Authorize]
    [ApiController]
    public class RequestOrderController : GamificationController
    {
        private readonly IRequestOrderService _requestOrderService;
        private readonly IValidator<CreateRequestOrderModel> _requestOrderModelValidator;

        public RequestOrderController
        (
            IRequestOrderService requestOrderService,
            IValidator<CreateRequestOrderModel> requestOrderModelValidator
        )
        {
            _requestOrderService = requestOrderService;
            _requestOrderModelValidator = requestOrderModelValidator;
        }

        /// <summary>
        /// Create new request order
        /// </summary>
        /// <response code="200">When request success sended and added</response>
        /// <response code="401">If the user doesn't have enough Xp</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        public async Task<IActionResult> AddRequestAsync([FromBody] CreateRequestOrderModel model)
        {
            var resultValidation = await _requestOrderModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var result = await _requestOrderService.AddAsync(model, UserId);
            if (result.Type == GamificationEnums.ResponseType.NotAllowed)
            {
                return Unauthorized();
            }

            return Ok();
        }

        /// <summary>
        /// Returns all order requests 
        /// </summary>
        /// <response code="200">Return all requests</response>
        /// <response code="403">When user don't have permissions to this action</response>
        [HttpGet]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> GetAllOrderRequests()
        {
            return Ok(await _requestOrderService.GetAllAsync());
        }

        /// <summary>
        /// Decline request
        /// </summary>
        /// <response code="200">Decline achievement request with current Id</response>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When request with current Id is not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<IActionResult> DeclineRequest(Guid id)
        {
            var requestOrder = await _requestOrderService.GetByIdAsync(id);
            if (requestOrder == null)
            {
                return NotFound();
            }

            await _requestOrderService.DeclineOrderRequestAsync(id);
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
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            var requestOrder = await _requestOrderService.GetByIdAsync(id);
            if (requestOrder == null)
            {
                return NotFound();
            }

            await _requestOrderService.ApproveOrderRequestAsync(id);

            return Ok();
        }
    }
}
