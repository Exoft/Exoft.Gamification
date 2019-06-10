using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : GamificationController
    {
        private IAchievementService achievementService;
        private UnitOfWork unitOfWork;
        private IMapper mapper;

        public AchievementController(IAchievementService achievementService, UnitOfWork unitOfWork, IMapper mapper)
        {
            this.achievementService = achievementService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        

        [HttpPost("add")]
        public async Task<IActionResult> AddAchievement([FromBody] NewAchievementModel model)
        {
            try
            {
                var achievement = mapper.Map<Achievement>(model);

                await achievementService.AddAsync(achievement);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAchievement([FromBody] DeleteAchievementModel model)
        {
            try
            {
                Guid guid = new Guid();
                if(!Guid.TryParse(model.Id, out guid))
                {
                    throw new FormatException("Value is not Guid");
                }

                Achievement achievement = await achievementService.GetAchievementAsync(guid);
                if(achievement == null)
                {
                    throw new NullReferenceException();
                }

                await achievementService.DeleteAsync(achievement);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponseModel(ex));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAchievement([FromBody] UpdateAchievementModel model)
        {
            try
            {
                Achievement achievement = await achievementService.GetAchievementAsync(model.Id);
                if (achievement == null)
                {
                    throw new NullReferenceException();
                }

                Achievement newAchievement = mapper.Map<Achievement>(model);

                await achievementService.UpdateAsync(newAchievement);
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