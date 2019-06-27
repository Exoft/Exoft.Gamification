using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/thanks")]
    [Authorize]
    [ApiController]
    public class ThanksController : GamificationController
    {
        private readonly IThankService _thankService;

        public ThanksController(IThankService thankService)
        {
            _thankService = thankService;
        }

        [HttpPost]
        public async Task<IActionResult> SayThanks([FromBody] CreateThankModel model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            await _thankService.AddAsync(model, UserId);
            return Ok();
        }
    }
}