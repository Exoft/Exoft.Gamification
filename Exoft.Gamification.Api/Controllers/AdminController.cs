using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/admin")]
    [Authorize]
    [ApiController]
    public class AdminController : BaseAdminController
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersWithFullInfoAsync(new PagingInfo { CurrentPage = 1, PageSize = 100 });
            return Ok(users);
        }
    }
}
