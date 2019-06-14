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

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
            var allItems = await _userService.GetAllUserAsync(pagingInfo);

            return Ok(allItems);
        }

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