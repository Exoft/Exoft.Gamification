using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService
        (
            IUserRepository userRepository,
            IUserAchievementRepository userAchievementRepository,
            IAchievementRepository achievementRepository,
            IFileService fileService,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _userAchievementRepository = userAchievementRepository;
            _achievementRepository = achievementRepository;
            _fileService = fileService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model, CancellationToken cancellationToken)
        {
            var achievement = new Achievement()
            {
                Name = model.Name,
                Description = model.Description,
                XP = model.XP                
            };
            achievement.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, achievement.IconId, cancellationToken);

            await _achievementRepository.AddAsync(achievement, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task DeleteAchievementAsync(Guid id, CancellationToken cancellationToken)
        {
            var achievement = await _achievementRepository.GetByIdAsync(id, cancellationToken);

            _achievementRepository.Delete(achievement);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ReadAchievementModel> GetAchievementByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var achievement = await _achievementRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ReadAchievementModel> UpdateAchievementAsync(UpdateAchievementModel model, Guid id, CancellationToken cancellationToken)
        {
            var achievement = await _achievementRepository.GetByIdAsync(id, cancellationToken);
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
                var userAchievements = await _userAchievementRepository.GetAllUsersByAchievementAsync(pagingInfo, id, cancellationToken);
                foreach (var userAchievement in userAchievements.Data)
                {
                    if (userAchievement.Achievement.Id == id)
                    {
                        var user = await _userRepository.GetByIdAsync(userAchievement.User.Id, cancellationToken);
                        user.XP -= difference;
                        user.XP = user.XP >= 0 ? user.XP : 0;

                        _userRepository.Update(user);

                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            achievement.XP = model.XP;
            achievement.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, achievement.IconId, cancellationToken);

            _achievementRepository.Update(achievement);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var page = await _achievementRepository.GetAllDataAsync(pagingInfo, cancellationToken);

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
