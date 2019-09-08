using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IAchievementService _achievementService;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAchievementService
        (
            IUserRepository userRepository,
            IAchievementRepository achievementRepository,
            IAchievementService achievementService,
            IUserAchievementRepository userAchievementsRepository,
            IEventRepository eventRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
            _achievementService = achievementService;
            _userAchievementRepository = userAchievementsRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(Guid userId, Guid achievementId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);

            var userAchievement = new UserAchievement()
            {
                User = user,
                Achievement = achievement
            };

            user.Achievements.Add(userAchievement);
            user.XP += achievement.XP;

            await _userAchievementRepository.AddAsync(userAchievement);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Got achievement ");
            stringBuilder.Append(achievement.Name);
            var eventEntity = new Event()
            {
                Description = stringBuilder.ToString(),
                User = user,
                Type = GamificationEnums.EventType.Records
            };
            await _eventRepository.AddAsync(eventEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

            var user = await _userRepository.GetByIdAsync(userAchievement.User.Id);

            _userAchievementRepository.Delete(userAchievement);

            user.Achievements.Remove(userAchievement);
            user.XP -= userAchievement.Achievement.XP;


            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId)
        {
            var countAchievements = await _userAchievementRepository.GetCountAchievementsByUserAsync(userId);
            var summaryXP = await _userAchievementRepository.GetSummaryXpByUserAsync(userId);
            var countAchievementsByThisMonth = await _userAchievementRepository.GetCountAchievementsByThisMonthAsync(userId);

            return new AchievementsInfoModel()
            {
                TotalXP = summaryXP,
                TotalAchievements = countAchievements,
                TotalAchievementsByThisMonth = countAchievementsByThisMonth
            };
        }

        public async Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId)
        {
            var page = await _userAchievementRepository
                .GetAllAchievementsByUserAsync(pagingInfo, userId);

            var readAchievementModel = page.Data.Select(i => _mapper.Map<ReadUserAchievementModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadUserAchievementModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readAchievementModel
            };

            return result;
        }

        public async Task<ReadUserAchievementModel> GetSingleUserAchievementAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetSingleUserAchievementAsync(userAchievementId);

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task<ReadUserAchievementModel> GetUserAchievementsByIdsAsync(Guid[] achievemetnsIds)
        {
            var userAchievement = await _userAchievementRepository.GetBy(x => achievemetnsIds.Any(y => y == x.Id)).ToListAsync();

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task AddOrUpdateAchievementsRangeAsync(Guid userId, IEnumerable<Guid> achievementsIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var allAchievements = (await _achievementRepository.GetBy(x=>achievementsIds.Any(y=>y==x.Id)).ToListAsync());
            var usersAchievements = (await _userAchievementRepository.GetAllAchievementsByUserAsync(new PagingInfo(), userId)).Data.ToList();

            var newUserAchievements = allAchievements.Where(x => usersAchievements.All(y => y.Achievement.Id != x.Id)).Select(x => new UserAchievement()
            {
                Achievement = x,
                User = user
            }).ToList();

            foreach (var item in newUserAchievements)
            {
                user.Achievements.Add(item);
                user.XP += item.Achievement.XP;
            }


            if (newUserAchievements.Count != 0)
                _userAchievementRepository.AddRangeAsync(newUserAchievements);
            //if (existingModels.Count != 0)
            //    _userAchievementRepository.UpdateRange(existingModels);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
