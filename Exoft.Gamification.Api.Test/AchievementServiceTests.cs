using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
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
    public class AchievementServiceTests
    {
        private Mock<IFileRepository> _fileRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock <IUserAchievementRepository> _userAchievementRepository;
        private Mock<IAchievementRepository> _achievementRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private AchievementService _achievementService;

        [SetUp]
        public void SetUp()
        {
            _fileRepository = new Mock<IFileRepository>();
            _achievementRepository = new Mock<IAchievementRepository>();
            _userRepository = new Mock<IUserRepository>();
            _userAchievementRepository = new Mock<IUserAchievementRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _achievementService = new AchievementService( _userRepository.Object, _userAchievementRepository.Object, _achievementRepository.Object, _fileRepository.Object,
                _mapper, _unitOfWork.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateAchievementModels))]
        public async Task AddAchievementAsync_CreateAchievementModel_ReturnsReadAchievementModel(CreateAchievementModel model)
        {
            //Arrange
            var expectedValue = AchievementDumbData.GetReadAchievementModel(model);

            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);
            _achievementRepository.Setup(x => x.AddAsync(It.IsAny<Achievement>())).Returns(Task.FromResult(expectedValue));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _achievementService.AddAchievementAsync(model);
            expectedValue.Id = response.Id;

            // Assert
            //_fileRepository.Verify(x => x.AddAsync(It.IsAny<File>()), Times.Once);
            _achievementRepository.Verify(x => x.AddAsync(It.IsAny<Achievement>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task DeleteAchievementAsync()
        {
            //Arrange
            var achievement = AchievementDumbData.GetRandomEntity();

            _achievementRepository.Setup(x => x.Delete(It.IsAny<Achievement>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _achievementService.DeleteAchievementAsync(achievement.Id);

            // Assert
            _achievementRepository.Verify(x => x.Delete(It.IsAny<Achievement>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase]
        public async Task GetAchievementByIdAsync()
        {
            //Arrange
            var achievement = AchievementDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadAchievementModel>(achievement);

            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(achievement));

            // Act
            var response = await _achievementService.GetAchievementByIdAsync(achievement.Id);

            // Assert
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateAchievementModels))]
        public async Task UpdateAchievementAsync_UpdateAchievementModel_ReturnsReadAchievementModel(UpdateAchievementModel model)
        {
            //Arrange
            var achievement = AchievementDumbData.GetEntity(model);
            var user = UserDumbData.GetRandomEntity();
            var userAchievements = UserAchievementDumbData.GetRandomEntities(5, user, achievement);
            var returnPagingInfo = ReturnPagingInfoDumbData.GetForModel(new PagingInfo(), userAchievements);
            var expectedValue = _mapper.Map<ReadAchievementModel>(achievement);

            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(achievement));
            _userAchievementRepository.Setup(x => x.GetAllUsersByAchievementAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>())).Returns(Task.FromResult(returnPagingInfo));
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            _achievementRepository.Setup(x => x.Update(It.IsAny<Achievement>()));
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _achievementService.UpdateAchievementAsync(model, achievement.Id);

            // Assert
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _achievementRepository.Verify(x => x.Update(It.IsAny<Achievement>()), Times.Once);
            //_fileRepository.Verify(x => x.AddAsync(It.IsAny<File>()), Times.Once);
            //_fileRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.PagingInfos))]
        public async Task GetAllAchievementsAsync(PagingInfo pagingInfo)
        {
            //Arrange
            var achievements = AchievementDumbData.GetRandomEntities(5);
            var paging = ReturnPagingInfoDumbData.GetForModel(pagingInfo, achievements);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadAchievementModel, Achievement>(paging, _mapper);

            _achievementRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).Returns(Task.FromResult(paging));

            // Act
            var response = await _achievementService.GetAllAchievementsAsync(pagingInfo);

            // Assert
            _achievementRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}
