using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AchievementService
        (
            IAchievementRepository achievementRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _achievementRepository = achievementRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //AZL I'd recommend you to have different models for create/update as far as update and create fields could be different
        public async Task AddAchievementAsync(InAchievementModel model)
        {
            await _achievementRepository.AddAsync(_mapper.Map<Achievement>(model));
        }

        public void DeleteAchievement(Guid Id)
        {
            //AZ: I've already commented you such lines.... DO NOT wait async methods
            var achievement = _achievementRepository.GetByIdAsync(Id).Result;

            _achievementRepository.Delete(achievement);
        }

        public async Task<OutAchievementModel> GetAchievementByIdAsync(Guid Id)
        {
            var achievement = await _achievementRepository.GetByIdAsync(Id);

            return _mapper.Map<OutAchievementModel>(achievement);
        }

        public async Task<ICollection<OutAchievementModel>> GetAchievementsDyUserAsync(Guid Id)
        {
            var achievements = await _userRepository.GetAchievementsByUserAsync(Id);

            return achievements.Select(i => _mapper.Map<OutAchievementModel>(i)).ToList();
        }

        //AZ: we dont need method in service that returns all items
        public async Task<ICollection<OutAchievementModel>> GetAllAsync()
        {
            var list = await _achievementRepository.GetAllAsync();

            return list.Select(i => _mapper.Map<OutAchievementModel>(i)).ToList();
        }

        public void UpdateAchievement(InAchievementModel model, Guid Id)
        {
            var achievement = _mapper.Map<Achievement>(model);
            achievement.Id = Id;

            _achievementRepository.Update(achievement);
        }
    }
}
