using System;
using System.Linq;
using System.Threading;
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

        public async Task AddAsync(Guid userId, Guid achievementId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            var achievement = await _achievementRepository.GetByIdAsync(achievementId, cancellationToken);

            var userAchievement = new UserAchievement()
            {
                User = user,
                Achievement = achievement
            };

            user.XP += achievement.XP;

            _userRepository.Update(user);

            user.Achievements.Add(userAchievement);

            await _userAchievementRepository.AddAsync(userAchievement, cancellationToken);

            await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid userAchievementsId, CancellationToken cancellationToken)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementsId, cancellationToken);

            var user = await _userRepository.GetByIdAsync(userAchievement.User.Id, cancellationToken);

            user.XP -= userAchievement.Achievement.XP;
            user.XP = user.XP >= 0 ? user.XP : 0;

            _userRepository.Update(user);

            user.Achievements.Add(userAchievement);

            _userAchievementRepository.Delete(userAchievement);

            user.Achievements.Remove(userAchievement);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AchievementsInfoModel> GetAchievementsInfoByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            var countAchievements = await _userAchievementRepository.GetCountAchievementsByUserAsync(userId, cancellationToken);
            var countAchievementsByThisMonth = await _userAchievementRepository.GetCountAchievementsByThisMonthAsync(userId, cancellationToken);

            return new AchievementsInfoModel()
            {
                TotalXP = user.XP,
                TotalAchievements = countAchievements,
                TotalAchievementsByThisMonth = countAchievementsByThisMonth
            };
        }

        public async Task<ReturnPagingInfo<ReadUserAchievementModel>> GetAllAchievementsByUserAsync(PagingInfo pagingInfo, Guid userId, CancellationToken cancellationToken)
        {
            var page = await _userAchievementRepository
                .GetAllAchievementsByUserAsync(pagingInfo, userId, cancellationToken);

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

        public async Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementsId, CancellationToken cancellationToken)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementsId, cancellationToken);

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task ChangeUserAchievementsAsync(AssignAchievementsToUserModel model, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            var page = new PagingInfo
            {
                CurrentPage = 1,
                PageSize = 0
            };
            var currentAchievements = (await _userAchievementRepository.GetAllAchievementsByUserAsync(page, userId, cancellationToken)).Data.ToList();
            var achievementsGroups = currentAchievements.GroupBy(i => i.Achievement.Id).ToArray();

            foreach (var achievementWithCount in model.Achievements)
            {
                var achievementsGroup = achievementsGroups
                    .FirstOrDefault(i => i.Key == achievementWithCount.AchievementId);

                if (achievementsGroup == null)
                {
                    for (var i = 0; i < achievementWithCount.Count; i++)
                    {
                        var achievement = await _achievementRepository.GetByIdAsync(achievementWithCount.AchievementId, cancellationToken);
                        await AddAchievementToUser(achievement, user, cancellationToken);
                        await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records, cancellationToken);
                    }
                }
                else
                {
                    for (var i = achievementsGroup.Count(); i < achievementWithCount.Count; i++)
                    {
                        var achievement = achievementsGroup.First().Achievement;
                        await AddAchievementToUser(achievement, user, cancellationToken);
                        await CreateEventAsync(user, $"Got achievement {achievement.Name}", GamificationEnums.EventType.Records, cancellationToken);
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

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task AddAchievementToUser(Achievement achievement, User user, CancellationToken cancellationToken)
        {
            var userAchievement = new UserAchievement
            {
                User = user,
                Achievement = achievement
            };

            user.XP += achievement.XP;

            _userRepository.Update(user);

            await _userAchievementRepository.AddAsync(userAchievement, cancellationToken);
        }

        private async Task CreateEventAsync(User user, string text, GamificationEnums.EventType type, CancellationToken cancellationToken)
        {
            var eventEntity = new Event()
            {
                Description = text,
                User = user,
                Type = type
            };
            await _eventRepository.AddAsync(eventEntity, cancellationToken);
        }
    }
}
