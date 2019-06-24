using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/events")]
    [Authorize]
    [ApiController]
    public class EventsController : GamificationController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get paged list of events
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of events</responce> 
        [HttpGet]
        public async Task<IActionResult> GetEventsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var list = await _eventService.GetAllEventAsync(pagingInfo);

            return Ok(list);
        }
    }
}