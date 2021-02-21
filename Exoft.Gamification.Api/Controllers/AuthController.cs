using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/authenticate")]
    [Authorize]
    [ApiController]
    public class AuthController : GamificationController
    {
        private readonly IAuthService _authService;
        private readonly IValidator<ResetPasswordModel> _resetPasswordModelValidator;
        private readonly IValidator<RequestResetPasswordModel> _requestResetPasswordModelValidator;

        public AuthController
        (
            IAuthService authService,
            IValidator<ResetPasswordModel> resetPasswordModel,
            IValidator<RequestResetPasswordModel> requestResetPasswordModelValidator
        )
        {
            _authService = authService;
            _resetPasswordModelValidator = resetPasswordModel;
            _requestResetPasswordModelValidator = requestResetPasswordModelValidator;
        }

        /// <summary>
        /// Get JWT
        /// </summary>
        /// <responce code="200">Return token</responce>
        /// <responce code="401">When userName or password is incorrect</responce>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginModel userModel, CancellationToken cancellationToken)
        {
            var user = await _authService.AuthenticateAsync(userModel.UserName, userModel.Password, cancellationToken);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        /// <summary>
        /// Get JWT
        /// </summary>
        /// <responce code="200">Return token</responce>
        /// <responce code="401">When email or password is incorrect</responce>
        [Obsolete("Wll remove in next release.", false)]
        [AllowAnonymous]
        [HttpPost("by-email")]
        public async Task<IActionResult> AuthenticateByEmailAsync([FromBody] UserLoginByEmailModel userModel, CancellationToken cancellationToken)
        {
            var user = await _authService.AuthenticateByEmailAsync(userModel.Email, userModel.Password, cancellationToken);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        /// <summary>
        /// Get new JWT
        /// </summary>
        /// <responce code="200">Return new token</responce>
        /// <responce code="401">When userName or password is incorrect</responce>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var newToken = await _authService.RefreshTokenAsync(model.RefreshToken, cancellationToken);

            if (newToken == null)
            {
                return Unauthorized();
            }

            return Ok(newToken);
        }

        /// <summary>
        /// Send recover password link to email
        /// </summary>
        /// <responce code="200">If email successful sent</responce>
        /// <responce code="400">When email null or empty</responce>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] RequestResetPasswordModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _requestResetPasswordModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var result = await _authService.SendForgotPasswordAsync(model.Email, model.ResetPasswordPageLink, cancellationToken);
            if (result.Type == Data.Core.Helpers.GamificationEnums.ResponseType.NotFound)
            {
                return NotFound(result.Message);
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
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _resetPasswordModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var result = await _authService.ResetPasswordAsync(model.SecretString, model.Password, cancellationToken);
            if (result.Type == Data.Core.Helpers.GamificationEnums.ResponseType.NotFound)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}