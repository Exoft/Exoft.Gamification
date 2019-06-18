using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    //[Authorize]
    [ApiController]
    public class UsersController : GamificationController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get paged list of users
        /// </summary>
        /// <responce code="200">Return the PageModel: pageNumber, pageSize and page of users</responce> 
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] PagingInfo pagingInfo)
        {
            var allItems = await _userService.GetAllUserAsync(pagingInfo);

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
            var item = await _userService.GetUserByIdAsync(userId);
            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <responce code="201">Return created user</responce> 
        /// <response code="422">When the model structure is correct but validation fails</response>
        [HttpPost]
        // TODO fix
        public async Task<IActionResult> AddUserAsync([FromForm] CreateUserModel model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var user = await _userService.AddUserAsync(model);

            return CreatedAtRoute(
                "GetUser",
                new { userId = user.Id },
                user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <responce code="200">Return the updated user</responce> 
        /// <responce code="404">When the user does not exist</responce> 
        /// <responce code="422">When the model structure is correct but validation fails</responce> 
        [HttpPut("{userId}")]
        // TODO fix
        public async Task<IActionResult> UpdateUserAsync([FromForm] UpdateUserModel model, Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
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
        /// <response code="404">When the user does not exist</response>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(userId);

            return NoContent();
        }
    }
}