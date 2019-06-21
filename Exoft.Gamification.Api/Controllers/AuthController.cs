using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/authenticate")]
    [Authorize]
    [ApiController]
    public class AuthController : GamificationController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserLoginModel userModel)
        {
            var user = await _authService.Authenticate(userModel.UserName, userModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }
    }
}