using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : GamificationController
    {
        private IUserService userService;
        private UnitOfWork unitOfWork;
        private IMapper mapper;

        public UsersController(IUserService userService, UnitOfWork unitOfWork, IMapper mapper)
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        // Test method
        [HttpPost("{id}")]
        public async Task<IActionResult> GetUserByUserName([FromBody] UserLoginModel model)
        {
            try
            {
                User user = await userService.GetUserAsync(model.UserName);
                if(user == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorResponseModel("User is not found!"));
                }

                UserModel userModel = mapper.Map<UserModel>(user);

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] NewUserModel model)
        {
            try
            {
                var user = mapper.Map<User>(model);

                await userService.AddAsync(user);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            try
            {
                Guid guid = new Guid();
                if (!Guid.TryParse(model.Id, out guid))
                {
                    throw new FormatException("Value is not Guid");
                }
                User user = await userService.GetUserAsync(guid);
                if(user == null)
                {
                    throw new NullReferenceException();
                }

                await userService.DeleteAsync(user);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model)
        {
            try
            {
                User user = await userService.GetUserAsync(model.Id);
                if (user == null)
                {
                    throw new NullReferenceException();
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Status = model.Status;
                user.Avatar = model.Avatar;
                user.Password = model.Password;

                await userService.UpdateAsync(user);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }
    }
}