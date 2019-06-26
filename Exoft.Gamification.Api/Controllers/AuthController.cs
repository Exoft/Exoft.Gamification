using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        /// <summary>
        /// Get jwt
        /// </summary>
        /// <responce code="200">Return token</responce>
        /// <responce code="401">When userName or password is incorrent</responce>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginModel userModel)
        {
            var user = await _authService.AuthenticateAsync(userModel.UserName, userModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        /// <summary>
        /// Get new jwt
        /// </summary>
        /// <responce code="200">Return new token</responce>
        /// <responce code="401">When userName or password is incorrent</responce>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromQuery] string refreshToken)
        {
            var newToken = await _authService.RefreshTokenAsync(refreshToken);
            
            if (newToken == null)
            {
                return Unauthorized();
            }
            
            return Ok(newToken);
        }
    }
}