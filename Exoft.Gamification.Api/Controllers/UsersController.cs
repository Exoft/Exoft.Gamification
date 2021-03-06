﻿using System;
using System.Threading;
using System.Threading.Tasks;

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
    public class UsersController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserModel> _createUserModelValidator;
        private readonly IValidator<UpdateFullUserModel> _updateFullUserModelValidator;
        private readonly IValidator<PagingInfo> _pagingInfoValidator;

        public UsersController
        (
            IUserService userService,
            IValidator<CreateUserModel> createUserModelValidator,
            IValidator<UpdateFullUserModel> updateFullUserModelValidator,
            IValidator<PagingInfo> pagingInfoValidator
        )
        {
            _userService = userService;
            _createUserModelValidator = createUserModelValidator;
            _updateFullUserModelValidator = updateFullUserModelValidator;
            _pagingInfoValidator = pagingInfoValidator;
        }

        /// <summary>
        /// Get paged list of users with short info
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of users</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpGet("with-short-info")]
        public async Task<IActionResult> GetUsersShortInfoAsync([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var allItems = await _userService.GetAllUsersWithShortInfoAsync(pagingInfo, cancellationToken);

            return Ok(allItems);
        }

        /// <summary>
        /// Get paged list of users with full info
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of users</responce>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpGet("with-full-info")]
        public async Task<IActionResult> GetUsersFullInfoAsync([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var allItems = await _userService.GetAllUsersWithFullInfoAsync(pagingInfo, cancellationToken);

            return Ok(allItems);
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <responce code="200">Return some user</responce> 
        /// <responce code="404">When user does not exist</responce> 
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var item = await _userService.GetFullUserByIdAsync(userId, cancellationToken);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <responce code="201">Return created user</responce> 
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="422">When the model structure is correct but validation fails</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromForm] CreateUserModel model, CancellationToken cancellationToken)
        {
            var resultValidation = await _createUserModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var user = await _userService.AddUserAsync(model, cancellationToken);

            return CreatedAtRoute(
                "GetUser",
                new { userId = user.Id },
                user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <responce code="200">Return the updated user</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <responce code="404">When the user does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync([FromForm] UpdateFullUserModel model, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetFullUserByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateFullUserModelValidator.ValidateAsync(model, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _userService.UpdateUserAsync(model, userId, cancellationToken);

            return Ok(item);
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <responce code="204">When the user successful delete</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When the user does not exist</response>
        [Authorize(Roles = GamificationRole.Admin)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetFullUserByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(userId, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <responce code="200">Return users</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [HttpGet("get-all")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> GetAllUsers([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var resultValidation = await _pagingInfoValidator.ValidateAsync(pagingInfo, cancellationToken);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var users = await _userService.GetAllUsersWithFullInfoAsync(pagingInfo, cancellationToken);

            return Ok(users);
        }
    }
}