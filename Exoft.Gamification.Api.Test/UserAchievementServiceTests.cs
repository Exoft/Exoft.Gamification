using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Test
{
    [TestFixture]
    public class UserAchievementServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IEventRepository> _eventRepository;
        private Mock<IAchievementRepository> _achievementRepository;
        private Mock<IUserAchievementRepository> _userAchievementRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private IUserAchievementService _userAchievementService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _eventRepository = new Mock<IEventRepository>();
            _achievementRepository = new Mock<IAchievementRepository>();
            _userAchievementRepository = new Mock<IUserAchievementRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _unitOfWork = new Mock<IUnitOfWork>();

            _userAchievementService = new UserAchievementService(
                _userRepository.Object, _achievementRepository.Object, _userAchievementRepository.Object,
                _eventRepository.Object, _unitOfWork.Object, _mapper);
        }

        public async Task AddAsync_ValidUserId_AchievementId()
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var achievement = AchievementDumbData.GetRandomEntity();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(achievement);
            _userAchievementRepository.Setup(x => x.AddAsync(It.IsAny<UserAchievement>())).Returns(Task.CompletedTask);
            _eventRepository.Setup(x => x.AddAsync(It.IsAny<Event>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _userAchievementService.AddAsync(user.Id, achievement.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.AddAsync(It.IsAny<UserAchievement>()), Times.Once);
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _eventRepository.Verify(x => x.AddAsync(It.IsAny<Event>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task DeleteAsync_ValidUserAchievementId(Guid userAchievementId)
        {
            //Arrange
            var userAchievement = UserAchievementDumbData.GetRandomEntity();
            var user = userAchievement.User;
            var achievement = userAchievement.Achievement;

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _userAchievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(userAchievement);
            _userAchievementRepository.Setup(x => x.Delete(It.IsAny<UserAchievement>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _userAchievementService.DeleteAsync(userAchievementId);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.Delete(It.IsAny<UserAchievement>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task GetAchievementsInfoByUserAsync_ValidUserId(Guid userId)
        {
            //Arrange
            var userAchievement = UserAchievementDumbData.GetRandomEntity();

            var countAchievementsByUser = RandomHelper.GetRandomNumber();
            var countAchievementsByMonth = RandomHelper.GetRandomNumber();
            var expectedValue = new AchievementsInfoModel
            {
                TotalAchievements = countAchievementsByUser,
                TotalXP = userAchievement.User.XP,
                TotalAchievementsByThisMonth = countAchievementsByMonth
            };

            _userAchievementRepository.Setup(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>())).ReturnsAsync(countAchievementsByUser);
            _userAchievementRepository.Setup(x => x.GetCountAchievementsByThisMonthAsync(It.IsAny<Guid>())).ReturnsAsync(countAchievementsByMonth);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(userAchievement.User));

            // Act
            var response = await _userAchievementService.GetAchievementsInfoByUserAsync(userId);

            // Assert
            _userAchievementRepository.Verify(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.GetCountAchievementsByThisMonthAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.PagingInfos))]
        public async Task GetAllAchievementsByUserAsync(PagingInfo pagingInfo)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var returnPage = ReturnPagingInfoDumbData.GetForModel(pagingInfo, UserAchievementDumbData.GetRandomEntities(5, user));
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadUserAchievementModel, UserAchievement>(returnPage, _mapper);

            _userAchievementRepository.Setup(x => x.GetAllAchievementsByUserAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>())).ReturnsAsync(returnPage);

            // Act
            var response = await _userAchievementService.GetAllAchievementsByUserAsync(pagingInfo, user.Id);

            // Assert
            _userAchievementRepository.Verify(x => x.GetAllAchievementsByUserAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>()), Times.Once);

            response.Should().BeEquivalentTo(expectedValue);
        }

        public async Task GetUserAchievementByIdAsync()
        {
            //Arrange
            var userAchievement = UserAchievementDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadUserAchievementModel>(userAchievement);

            _userAchievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(userAchievement);

            // Act
            var response = await _userAchievementService.GetUserAchievementByIdAsync(userAchievement.Id);

            // Assert
            _userAchievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            response.Should().BeEquivalentTo(expectedValue);
        }

        public async Task ChangeUserAchievementsAsync_ValidAssignAchievementsToUserModel(AssignAchievementsToUserModel model)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var userAchievements = UserAchievementDumbData.GetRandomEntities(5, user);
            var returnpagingInfo = ReturnPagingInfoDumbData.GetForModel(new PagingInfo(), userAchievements);

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _userAchievementRepository.Setup(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>())).ReturnsAsync(RandomHelper.GetRandomNumber());
            _userAchievementRepository.Setup(x => x.GetAllAchievementsByUserAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>())).Returns(Task.FromResult(returnpagingInfo));
            _userAchievementRepository.Setup(x => x.AddAsync(It.IsAny<UserAchievement>())).Returns(Task.CompletedTask);
            _userAchievementRepository.Setup(x => x.Delete(It.IsAny<UserAchievement>()));
            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(AchievementDumbData.GetRandomEntity()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _userAchievementService.ChangeUserAchievementsAsync(model, user.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.GetAllAchievementsByUserAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>()), Times.Once);
            _userAchievementRepository.Verify(x => x.AddAsync(It.IsAny<UserAchievement>()), Times.Once);
            _userAchievementRepository.Verify(x => x.Delete(It.IsAny<UserAchievement>()), Times.Once);
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
