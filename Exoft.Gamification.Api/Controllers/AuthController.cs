using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : GamificationController
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserLoginModel userModel)
        {
            var user = _authService.Authenticate(userModel.UserName, userModel.Password);

            if (user == null)
            {
                return BadRequest(new ErrorResponseModel("Username or password is incorrect"));
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSomeData()
        {
            return Ok("OK");
        }
    }
}