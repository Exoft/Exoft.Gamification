using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ReadUserAchievementModel> GetUserAchievementByIdAsync(Guid userAchievementId)
        {
            var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

            return _mapper.Map<ReadUserAchievementModel>(userAchievement);
        }

        public async Task ChangeUserAchievementsAsync(AssignAchievementsToUserModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var countOfAchievements = await _userAchievementRepository.GetCountAchievementsByUserAsync(userId);
            var page = new PagingInfo
            {
                CurrentPage = 1,
                PageSize = countOfAchievements
            };
            var currentAchievements = (await _userAchievementRepository.GetAllAchievementsByUserAsync(page, userId)).Data.ToList();

            var achievementsGroups = currentAchievements.GroupBy(i => i.Achievement.Id);
            foreach (var achievementWithCount in model.Achievements)
            {
                var achievementsGroup = achievementsGroups.FirstOrDefault(i => i.Key == achievementWithCount.AchievementId);

                if (achievementsGroup == null)
                {
                    for (int i = 0; i < achievementWithCount.Count; i++)
                    {
                        var achievement = await _achievementRepository.GetByIdAsync(achievementWithCount.AchievementId);
                        await AddAchievementToUser(achievement, user);
                    }
                }
                else
                {
                    for (int i = achievementsGroup.Count(); i < achievementWithCount.Count; i++)
                    {
                        var achievement = achievementsGroup.First().Achievement;
                        await AddAchievementToUser(achievement, user);
                    }

                    for (int i = achievementsGroup.Count(); i > achievementWithCount.Count; i--)
                    {
                        var lastAchievement = achievementsGroup.OrderByDescending(a => a.AddedTime).First();

                        _userAchievementRepository.Delete(lastAchievement);

                        user.XP -= lastAchievement.Achievement.XP;
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

            await _userAchievementRepository.AddAsync(userAchievement);

            user.XP += userAchievement.Achievement.XP;
        }
    }
}
