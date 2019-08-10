using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/push")]
    [Authorize]
    [ApiController]
    public class PushController : GamificationController
    {
        private readonly IValidator<PushRequestModel> _pushRequestModelValidator;
        private readonly IPushNotificationService _pushNotificationService;
        public PushController(IValidator<PushRequestModel> pushRequestModelValidator, IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
            _pushRequestModelValidator = pushRequestModelValidator;
        }

        /// <summary>
        /// Send push notification to mobile device
        /// </summary>
        /// <responce code="202">When response from AppCenter is false</responce> 
        /// <responce code="204">When notification sent succeful</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendNotification(PushRequestModel model)
        {
            var resultValidation = await _pushRequestModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var response = await _pushNotificationService.SendNotification(model);
            if (response.isSend) return NoContent();
            return Accepted();
        }
    }
}
