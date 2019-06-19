using AutoMapper;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class UserAchievementsService : IUserAchievementsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserAchievementsRepository _userAchievementsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAchievementsService
        (
            IUserRepository userRepository,
            IAchievementRepository achievementRepository,
            IUserAchievementsRepository userAchievementsRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
            _userAchievementsRepository = userAchievementsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(Guid userId, Guid achievementId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);

            var userAchievements = new UserAchievements()
            {
                User = user,
                Achievement = achievement
            };

            user.Achievements.Add(userAchievements);

            await _userAchievementsRepository.AddAsync(userAchievements);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userAchievementId)
        {
            var userAchievements = await _userAchievementsRepository.GetByIdAsync(userAchievementId);

            var user = await _userRepository.GetByIdAsync(userAchievements.User.Id);
            
            _userAchievementsRepository.Delete(userAchievements);

            user.Achievements.Remove(userAchievements);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReturnPagingInfo<ReadAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId)
        {
            var page = await _userAchievementsRepository
                .GetAllAchievementsByUserAsync(pagingInfo, userId);

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

        public async Task<ReadAchievementModel> GetSingleUserAchievementAsync(Guid userId, Guid achievementId)
        {
            var userAchievement = await _userAchievementsRepository.GetSingleUserAchievementAsync(userId, achievementId);

            return _mapper.Map<ReadAchievementModel>(userAchievement);
        }

        public async Task<ReadAchievementModel> GetUserAchievementsByIdAsync(Guid userAchievementsId)
        {
            var userAchievement = await _userAchievementsRepository.GetByIdAsync(userAchievementsId);

            return _mapper.Map<ReadAchievementModel>(userAchievement);
        }
    }
}
