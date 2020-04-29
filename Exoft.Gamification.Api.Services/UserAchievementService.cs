using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAchievementService
        (
            IUserRepository userRepository,
            IAchievementRepository achievementRepository,
            IUserAchievementRepository userAchievementsRepository,
            IEventRepository eventRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
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

            user.XP += achievement.XP;

            _userRepository.Update(user);

            user.Achievements.Add(userAchievement);

            await _userAchievementRepository.AddAsync(userAchievement);

            await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

            var user = await _userRepository.GetByIdAsync(userAchievement.User.Id);

            user.XP -= userAchievement.Achievement.XP;
            user.XP = user.XP >= 0 ? user.XP : 0;

            _userRepository.Update(user);

            user.Achievements.Add(userAchievement);

            _userAchievementRepository.Delete(userAchievement);

            user.Achievements.Remove(userAchievement);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var countAchievements = await _userAchievementRepository.GetCountAchievementsByUserAsync(userId);
            var countAchievementsByThisMonth = await _userAchievementRepository.GetCountAchievementsByThisMonthAsync(userId);

            return new AchievementsInfoModel()
            {
                TotalXP = user.XP,
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

        public async Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task ChangeUserAchievementsAsync(AssignAchievementsToUserModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var page = new PagingInfo
            {
                CurrentPage = 1,
                PageSize = 0
            };
            var currentAchievements = (await _userAchievementRepository.GetAllAchievementsByUserAsync(page, userId)).Data.ToList();
            var achievementsGroups = currentAchievements.GroupBy(i => i.Achievement.Id);

            foreach (var achievementWithCount in model.Achievements)
            {
                var achievementsGroup = achievementsGroups
                    .FirstOrDefault(i => i.Key == achievementWithCount.AchievementId);

                if (achievementsGroup == null)
                {
                    for (int i = 0; i < achievementWithCount.Count; i++)
                    {
                        var achievement = await _achievementRepository.GetByIdAsync(achievementWithCount.AchievementId);
                        await AddAchievementToUser(achievement, user);
                        await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records);
                    }
                }
                else
                {
                    for (int i = achievementsGroup.Count(); i < achievementWithCount.Count; i++)
                    {
                        var achievement = achievementsGroup.First().Achievement;
                        await AddAchievementToUser(achievement, user);
                        await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records);
                    }

                    for (int i = achievementsGroup.Count(), j = 0; i > achievementWithCount.Count; i--, j++)
                    {
                        var lastAchievement = achievementsGroup
                            .OrderByDescending(a => a.AddedTime)
                            .Skip(j)
                            .First();

                        lastAchievement.User.XP -= lastAchievement.Achievement.XP;
                        lastAchievement.User.XP = lastAchievement.User.XP >= 0 ? lastAchievement.User.XP : 0;

                        _userRepository.Update(lastAchievement.User);

                        _userAchievementRepository.Delete(lastAchievement);
                    }
                }
            }

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task AddAchievementToUser(Achievement achievement, User user)
        {
            var userAchievement = new UserAchievement
            {
                User = user,
                Achievement = achievement
            };

            user.XP += achievement.XP;

            _userRepository.Update(user);

            await _userAchievementRepository.AddAsync(userAchievement);
        }

        private async Task CreateEventAsync(User user, string text, GamificationEnums.EventType type)
        {
            var eventEntity = new Event()
            {
                Description = text,
                User = user,
                Type = type
            };
            await _eventRepository.AddAsync(eventEntity);
        }
    }
}
