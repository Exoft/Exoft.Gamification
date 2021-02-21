using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace Exoft.Gamification.Api.Test.ServiceTests
{
    [TestFixture]
    public class AchievementServiceTests
    {
        private Mock<IFileService> _fileService;
        private Mock<IUserRepository> _userRepository;
        private Mock <IUserAchievementRepository> _userAchievementRepository;
        private Mock<IAchievementRepository> _achievementRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private AchievementService _achievementService;

        [SetUp]
        public void SetUp()
        {
            _fileService = new Mock<IFileService>();
            _achievementRepository = new Mock<IAchievementRepository>();
            _userRepository = new Mock<IUserRepository>();
            _userAchievementRepository = new Mock<IUserAchievementRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _achievementService = new AchievementService( _userRepository.Object, _userAchievementRepository.Object, _achievementRepository.Object, _fileService.Object,
                _mapper, _unitOfWork.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateAchievementModels))]
        public async Task AddAchievementAsync_CreateAchievementModel_ReturnsReadAchievementModel(CreateAchievementModel model)
        {
            //Arrange
            var expectedValue = AchievementDumbData.GetReadAchievementModel(model);
            var cancellationToken = new CancellationToken();

            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult(expectedValue.IconId));
            _achievementRepository.Setup(x => x.AddAsync(It.IsAny<Achievement>(), cancellationToken)).Returns(Task.FromResult(expectedValue));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _achievementService.AddAchievementAsync(model, cancellationToken);
            expectedValue.Id = response.Id;

            // Assert
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _achievementRepository.Verify(x => x.AddAsync(It.IsAny<Achievement>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task DeleteAchievementAsync()
        {
            //Arrange
            var achievement = AchievementDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _achievementRepository.Setup(x => x.Delete(It.IsAny<Achievement>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _achievementService.DeleteAchievementAsync(achievement.Id, cancellationToken);

            // Assert
            _achievementRepository.Verify(x => x.Delete(It.IsAny<Achievement>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [TestCase]
        public async Task GetAchievementByIdAsync()
        {
            //Arrange
            var achievement = AchievementDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadAchievementModel>(achievement);
            var cancellationToken = new CancellationToken();

            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(achievement));

            // Act
            var response = await _achievementService.GetAchievementByIdAsync(achievement.Id, cancellationToken);

            // Assert
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
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
            var cancellationToken = new CancellationToken();

            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(achievement));
            _userAchievementRepository.Setup(x => x.GetAllUsersByAchievementAsync(It.IsAny<PagingInfo>(), It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(returnPagingInfo));
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(user));
            _achievementRepository.Setup(x => x.Update(It.IsAny<Achievement>()));
            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult(expectedValue.IconId));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _achievementService.UpdateAchievementAsync(model, achievement.Id, cancellationToken);

            // Assert
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _achievementRepository.Verify(x => x.Update(It.IsAny<Achievement>()), Times.Once);
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllAchievementsAsync(PagingInfo pagingInfo)
        {
            //Arrange
            var achievements = AchievementDumbData.GetRandomEntities(5);
            var paging = ReturnPagingInfoDumbData.GetForModel(pagingInfo, achievements);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadAchievementModel, Achievement>(paging, _mapper);
            var cancellationToken = new CancellationToken();

            _achievementRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(paging));

            // Act
            var response = await _achievementService.GetAllAchievementsAsync(pagingInfo, cancellationToken);

            // Assert
            _achievementRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}
