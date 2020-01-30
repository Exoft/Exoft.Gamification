using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : GamificationController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IValidator<CreateUserModel> _createUserModelValidator;
        private readonly IValidator<UpdateFullUserModel> _updateFullUserModelValidator;

        public UsersController
        (
            IUserService userService,
            IRoleService roleService,
            IValidator<CreateUserModel> createUserModelValidator,
            IValidator<UpdateFullUserModel> updateFullUserModelValidator
        )
        {
            _userService = userService;
            _roleService = roleService;
            _createUserModelValidator = createUserModelValidator;
            _updateFullUserModelValidator = updateFullUserModelValidator;
        }

        /// <summary>
        /// Get paged list of users with short info
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of users</responce> 
        [HttpGet("with-short-info")]
        public async Task<IActionResult> GetUsersShortInfoAsync([FromQuery] PagingInfo pagingInfo)
        {
            var allItems = await _userService.GetAllUsersWithShortInfoAsync(pagingInfo);

            return Ok(allItems);
        }

        /// <summary>
        /// Get paged list of users with full info
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of users</responce> 
        [HttpGet("with-full-info")]
        public async Task<IActionResult> GetUsersFullInfoAsync([FromQuery] PagingInfo pagingInfo)
        {
            var allItems = await _userService.GetAllUsersWithFullInfoAsync(pagingInfo);

            return Ok(allItems);
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <responce code="200">Return some user</responce> 
        /// <responce code="404">When user does not exist</responce> 
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId)
        {
            var item = await _userService.GetFullUserByIdAsync(userId);
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
        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromForm] CreateUserModel model)
        {
            var resultValidation = await _createUserModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (_roleService.CalculateAllowOperationsByUsersRole(User, model.Roles))
            {
                var user = await _userService.AddUserAsync(model);

                return CreatedAtRoute(
                    "GetUser",
                    new { userId = user.Id },
                    user);
            }
            return Forbid();
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <responce code="200">Return the updated user</responce> 
        /// <responce code="404">When the user does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync([FromForm] UpdateFullUserModel model, Guid userId)
        {
            var user = await _userService.GetFullUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultValidation = await _updateFullUserModelValidator.ValidateAsync(model);
            resultValidation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var item = await _userService.UpdateUserAsync(model, userId);

            return Ok(item);
        }

        /// <summary>
        /// Delete user by Id
        /// </summary>
        /// <responce code="204">When the user successful delete</responce>
        /// <response code="403">When user don't have permissions to this action</response>
        /// <response code="404">When the user does not exist</response>
        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userService.GetFullUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (_roleService.CalculateAllowOperationsByUsersRole(User, user.Roles))
            {
                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            return Forbid();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <responce code="200">Return users</responce> 
        [HttpGet("get-all")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> GetAllUsers([FromQuery] PagingInfo pagingInfo)
        {
            var users = await _userService.GetAllUsersWithFullInfoAsync(pagingInfo);

            return Ok(users);
        }
    }
}