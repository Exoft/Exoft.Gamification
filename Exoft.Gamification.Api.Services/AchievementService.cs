using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService
        (
            IAchievementRepository achievementRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _achievementRepository = achievementRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model)
        {
            var achievement = new Achievement()
            {
                Name = model.Name,
                Description = model.Description,
                XP = model.XP
            };

            if(model.Icon != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    model.Icon.CopyTo(memory);

                    achievement.Icon = new File()
                    {
                        Data = memory.ToArray()
                    };
                }
            }

            await _achievementRepository.AddAsync(achievement);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task DeleteAchievementAsync(Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);

            _achievementRepository.Delete(achievement);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReadAchievementModel> GetAchievementByIdAsync(Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);
            achievement.Name = model.Name;
            achievement.Description = model.Description;
            achievement.XP = model.XP;

            if (model.Icon != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    model.Icon.CopyTo(memory);

                    achievement.Icon = new File()
                    {
                        Data = memory.ToArray()
                    };

                }
            }
            _achievementRepository.Update(achievement);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ReturnPagingModel<ReadAchievementModel>> GetPagedAchievementAsync(InputPagingModel pagingModel)
        {
            var page = await _achievementRepository.GetPagingDataAsync(_mapper.Map<PagingInfo>(pagingModel));
            var readAchievementModel = new List<ReadAchievementModel>();
            foreach (var item in page.Data)
            {
                readAchievementModel.Add(_mapper.Map<ReadAchievementModel>(item));
            }
            var result = new ReturnPagingModel<ReadAchievementModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readAchievementModel
            };

            return result;
        }

        public async Task<ReturnPagingModel<ReadAchievementModel>> GetPagedAchievementByUserAsync(InputPagingModel pagingModel, Guid UserId)
        {
            var page = await _achievementRepository
                .GetPagedAchievementByUserAsync(_mapper.Map<PagingInfo>(pagingModel), UserId);

            var readAchievementModel = new List<ReadAchievementModel>();
            foreach (var item in page.Data)
            {
                readAchievementModel.Add(_mapper.Map<ReadAchievementModel>(item));
            }
            var result = new ReturnPagingModel<ReadAchievementModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readAchievementModel
            };

            return result;
        }

        public async Task<ReadAchievementModel> IsUserHaveAchievement(Guid userId, Guid achievementId)
        {
            var achievement = await _achievementRepository.IsUserHaveAchievementAsync(userId, achievementId);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }
    }
}
