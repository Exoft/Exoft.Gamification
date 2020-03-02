using AutoMapper;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService
        (
            IUserRepository userRepository,
            IUserAchievementRepository userAchievementRepository,
            IAchievementRepository achievementRepository,
            IFileRepository fileRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _userAchievementRepository = userAchievementRepository;
            _achievementRepository = achievementRepository;
            _fileRepository = fileRepository;
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

            if (model.Icon != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await model.Icon.CopyToAsync(memory);

                    var file = new File()
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Icon.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    achievement.IconId = file.Id;
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

        public async Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(id);
            achievement.Name = model.Name;
            achievement.Description = model.Description;

            var difference = achievement.XP - model.XP;
            if (difference != 0)
            {
                var pagingInfo = new PagingInfo
                {
                    CurrentPage = 1,
                    PageSize = 0
                };
                var userAchievements = await _userAchievementRepository.GetAllUsersByAchievementAsync(pagingInfo, id);
                foreach (var userAchievement in userAchievements.Data)
                {
                    if (userAchievement.Achievement.Id == id)
                    {
                        var user = await _userRepository.GetByIdAsync(userAchievement.User.Id);
                        user.XP -= difference;

                        _userRepository.Update(user);

                        await _unitOfWork.SaveChangesAsync();
                    }
                }
            }

            achievement.XP = model.XP;

            if (model.Icon != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await model.Icon.CopyToAsync(memory);

                    if (achievement.IconId != null)
                    {
                        await _fileRepository.Delete(achievement.IconId.Value);
                    }

                    var file = new File()
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Icon.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    achievement.IconId = file.Id;
                }
            }
            _achievementRepository.Update(achievement);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsAsync(PagingInfo pagingInfo)
        {
            var page = await _achievementRepository.GetAllDataAsync(pagingInfo);

            var readAchievementModel = page.Data.Select(i => _mapper.Map<ReadAchievementModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadAchievementModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readAchievementModel
            };

            return result;
        }
    }
}
