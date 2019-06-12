using AutoMapper;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AchievementService
        (
            IAchievementRepository achievementRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _achievementRepository = achievementRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<ReadAchievementModel> AddAchievementAsync(CreateAchievementModel model)
        {
            var achievement = await _achievementRepository.AddAsync(_mapper.Map<Achievement>(model));

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async void DeleteAchievementAsync(Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);

            _achievementRepository.Delete(achievement);
        }

        public async Task<ReadAchievementModel> GetAchievementByIdAsync(Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadAchievementModel>(achievement);
        }

        public async Task<ICollection<ReadAchievementModel>> GetUserAchievementsAsync(Guid Id)
        {
            var achievements = await _userRepository.GetAchievementsByUserAsync(Id);

            return achievements.Select(i => _mapper.Map<ReadAchievementModel>(i)).ToList();
        }
        
        public async Task<ICollection<ReadAchievementModel>> GetPagedAchievement(PageInfo pageInfo)
        {
            var list = await _achievementRepository.GetPagedAchievementAsync(pageInfo);

            return list.Select(i => _mapper.Map<ReadAchievementModel>(i)).ToList();
        }

        public async Task SaveChangesAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

        public ReadAchievementModel UpdateAchievement(UpdateAchievementModel model, Guid Id)
        {
            var achievement = _mapper.Map<Achievement>(model);
            achievement.Id = Id;

            var entity = _achievementRepository.Update(achievement);

            return _mapper.Map<ReadAchievementModel>(entity);
        }
    }
}
