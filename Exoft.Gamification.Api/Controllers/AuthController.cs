using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        private readonly IValidator<ResetPasswordModel> _resetPasswordModel;

        public AuthController
        (
            IAuthService authService,
            IValidator<ResetPasswordModel> resetPasswordModel
        )
        {
            _authService = authService;
            _resetPasswordModel = resetPasswordModel;
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

        /// <summary>
        /// Send recover password link to email
        /// </summary>
        /// <responce code="200">If email successful sended</responce>
        /// <responce code="400">When email null or empty</responce>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromQuery] string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            try
            {
                await _authService.ForgotPasswordAsync(email);
            }
            catch (System.Exception)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Set new password for user
        /// </summary>
        /// <responce code="200">If password successful update</responce>
        /// <responce code="400">When newPassword or secretString null or empty</responce>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordModel model)
        {
            var resultValidation = await _resetPasswordModel.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _authService.ResetPasswordAsync(model.SecretString, model.Password);

            return Ok();
        }
    }
}