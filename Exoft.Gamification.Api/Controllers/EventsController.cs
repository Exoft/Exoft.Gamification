using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : GamificationController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventsAsync([FromQuery] PagingInfo pagingInfo)
        {
            var list = await _eventService.GetAllEventAsync(pagingInfo);

            return Ok(list);
        }
    }
}