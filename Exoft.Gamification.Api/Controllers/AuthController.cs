using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Authorize]
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : GamificationController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Get jwt
        /// </summary>
        /// <responce code="200">Return token</responce>
        /// <responce code="401">When userName or password is incorrent</responce>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromForm] UserLoginModel userModel)
        {
            var token = await _authService.Authenticate(userModel.UserName, userModel.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        /// <summary>
        /// Get new jwt
        /// </summary>
        /// <responce code="200">Return new token</responce>
        /// <responce code="401">When userName or password is incorrent</responce>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromForm] RefreshTokenModel model)
        {
            var newToken = await _authService.RefreshTokenAsync(model);

            if(newToken == null)
            {
                return Unauthorized();
            }

            return Ok(newToken);
        }
    }
}