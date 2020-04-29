using System.Threading.Tasks;

using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/events")]
    [Authorize]
    [ApiController]
    public class EventsController : GamificationController
    {
        private readonly IEventService _eventService;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public EventsController
        (
            IEventService eventService,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _eventService = eventService;
            _pagingInfoValidator = pagingInfoValidator;
        }

        /// <summary>
        /// Get paged list of events
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of events</responce>
        /// <response code="422">When the model structure is correct but validation fails</response> 
        [HttpGet]
        public async Task<IActionResult> GetEventsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var list = await _eventService.GetAllEventAsync(pagingInfo);

            return Ok(list);
        }
    }
}