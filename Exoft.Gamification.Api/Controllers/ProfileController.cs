﻿using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class ProfileController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IUserAchievementService _userAchievementService;
        private readonly IValidator<UpdateUserModel> _updateUserModelValidator;
        private readonly IValidator<ChangePasswordModel> _changePasswordModelValidator;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public ProfileController
        (
            IUserService userService,
            IUserAchievementService userAchievementService,
            IValidator<UpdateUserModel> updateUserModelValidator,
            IValidator<ChangePasswordModel> changePasswordModelValidator,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _userService = userService;
            _userAchievementService = userAchievementService;
            _updateUserModelValidator = updateUserModelValidator;
            _changePasswordModelValidator = changePasswordModelValidator;
            _pagingInfoValidator = pagingInfoValidator;
        }

        /// <summary>
        /// Get summary info about current user
        /// </summary>
        /// <responce code="200">Return info about user</responce>
        /// <responce code="404">When user does not exist</responce>
        [HttpGet("current-user-info")]
        public async Task<IActionResult> GetCurrentUserInfo(CancellationToken cancellationToken)
        {
            var user = await _userService.GetShortUserByIdAsync(UserId, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Get all info about current user
        /// </summary>
        /// <responce code="200">Return info about user</responce>
        /// <responce code="404">When user does not exist</responce>
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var user = await _userService.GetFullUserByIdAsync(UserId, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <responce code="200">Return the updated user</responce> 
        /// <responce code="404">When the user does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [HttpPut("current-user")]
        public async Task<IActionResult> UpdateCurrentUser([FromForm] UpdateUserModel model, CancellationToken cancellationToken)
        {
            var user = await _userService.GetFullUserByIdAsync(UserId, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateUserModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            var newUser = await _userService.UpdateUserAsync(model, UserId, cancellationToken);

            return Ok(newUser);
        }

        /// <summary>
        /// Get paged list of achievements current user
        /// </summary>
        /// <responce code="200">Return all achievements current user</responce>
        /// <response code="422">When the model structure is correct but validation fails</response> 
        [HttpGet("current-user/achievements")]
        public async Task<IActionResult> GetUserAchievementsAsync([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _userAchievementService.GetAllAchievementsByUserAsync(pagingInfo, UserId, cancellationToken);

            return Ok(item);
        }

        /// <summary>
        /// Get info about achievements current user
        /// </summary>
        /// <responce code="200">Return info about achievements current user</responce> 
        [HttpGet("current-user/achievements/info")]
        public async Task<IActionResult> GetAchievementsInfo(CancellationToken cancellationToken)
        {
            var achievementsInfo = await _userAchievementService.GetAchievementsInfoByUserAsync(UserId, cancellationToken);

            return Ok(achievementsInfo);
        }

        /// <summary>
        /// Change password for user
        /// </summary>
        /// <responce code="200">If password successful changed</responce>
        /// <response code="422">When the model structure is correct but validation fails</response> 
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _changePasswordModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _userService.UpdatePasswordAsync(UserId, model.NewPassword, cancellationToken);
            return Ok();
        }
    }
}