﻿using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class ProfileController : GamificationController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get summary info about current user
        /// </summary>
        /// <responce code="200">Return info about user</responce>
        /// <responce code="404">When user does not exist</responce>
        [HttpGet("current-user-info")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var user = await _userService.GetShortUserByIdAsync(UserId);
            if(user == null)
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
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetFullUserByIdAsync(UserId);
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
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserModel model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            var user = await _userService.GetFullUserByIdAsync(UserId);
            if(user == null)
            {
                return NotFound();
            }

            var newUser = await _userService.UpdateUserAsync(model, UserId);

            return Ok(newUser);
        }
    }
}