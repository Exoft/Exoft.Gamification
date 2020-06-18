using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Test
{
    [TestFixture]
    public class RequestAchievementServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IAchievementRepository> _achievementRepository;
        private Mock<IStringLocalizer<HtmlPages>> _stringLocalizer;
        private Mock<IEmailService> _emailService;
        private Mock<IRequestAchievementRepository> _requestAchievementRepository;
        private Mock<IUserAchievementService> _userAchievementService;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private IRequestAchievementService _requestAchievementService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _achievementRepository = new Mock<IAchievementRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _unitOfWork = new Mock<IUnitOfWork>();

            _stringLocalizer = new Mock<IStringLocalizer<HtmlPages>>();
            _emailService = new Mock<IEmailService>();
            _requestAchievementRepository = new Mock<IRequestAchievementRepository>();
            _userAchievementService = new Mock<IUserAchievementService>();

            _requestAchievementService = new RequestAchievementService(_requestAchievementRepository.Object, _emailService.Object,
                _mapper, _userAchievementService.Object, _userRepository.Object, _achievementRepository.Object, _stringLocalizer.Object,
                _unitOfWork.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidRequestAchievementModels))]
        public async Task AddAsync_ValidRequestAchievementModel_ReturnsIResponse(CreateRequestAchievementModel model)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var achievement = AchievementDumbData.GetRandomEntity();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            _userRepository.Setup(x => x.GetAdminsEmailsAsync()).Returns(Task.FromResult(new List<string>() as ICollection<string>));
            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(achievement));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _emailService.Setup(x => x.SendEmailsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>())).Returns(Task.CompletedTask);
            _requestAchievementRepository.Setup(x => x.AddAsync(It.IsAny<RequestAchievement>())).Returns(Task.FromResult(_mapper.Map<RequestAchievement>(model)));

            string key = "RequestAchievementPage";
            var localizedString = new LocalizedString(key, key);
            _stringLocalizer.Setup(_ => _[key]).Returns(localizedString);

            // Act
            var response = await _requestAchievementService.AddAsync(model, user.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetAdminsEmailsAsync(), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _emailService.Verify(x => x.SendEmailsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
            _stringLocalizer.Verify(_ => _[key], Times.Once);
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _requestAchievementRepository.Verify(x => x.AddAsync(It.IsAny<RequestAchievement>()), Times.Once);

            response.Should().BeEquivalentTo(new OkResponse());
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidRequestAchievements))]
        public async Task DeleteAsync_ValidRequestAchievement(RequestAchievement achievement)
        {
            //Arrange
            _requestAchievementRepository.Setup(x => x.Delete(It.IsAny<RequestAchievement>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _requestAchievementService.DeleteAsync(achievement);

            // Assert
            _requestAchievementRepository.Verify(x => x.Delete(It.IsAny<RequestAchievement>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase]
        public async Task GetAllAsync_ReturnsReadRequestAchievementModels()
        {
            //Arrange
            int entitiesCount = 5;

            var achievements = AchievementDumbData.GetRandomEntities(entitiesCount);
            var users = UserDumbData.GetRandomEntities(entitiesCount);
            var listRequestAchievements = RequestAchievementDumbData.GetEntities(achievements, users);
            var pagingInfo = ReturnPagingInfoDumbData.GetForModel(new PagingInfo(), listRequestAchievements);

            var expectedValue = RequestAchievementDumbData.GetReadRequestAchievementModels(listRequestAchievements, achievements, users);
            

            _requestAchievementRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).Returns(Task.FromResult(pagingInfo));
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns((Guid x) => Task.FromResult(users.FirstOrDefault(y => y.Id == x)));
            _achievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns((Guid x) => Task.FromResult(achievements.FirstOrDefault(y => y.Id == x)));

            // Act
            var response = await _requestAchievementService.GetAllAsync();

            // Assert
            _requestAchievementRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(listRequestAchievements.Count));
            _achievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(listRequestAchievements.Count));

            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task GetByIdAsync()
        {
            var expectedRequestAchievement = RequestAchievementDumbData.GetRandomEntity();
            //Arrange
            _requestAchievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(expectedRequestAchievement));

            // Act
            var result = await _requestAchievementService.GetByIdAsync(expectedRequestAchievement.Id);

            // Assert
            _requestAchievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            result.Should().BeEquivalentTo(expectedRequestAchievement);
        }

        [TestCase]
        public async Task ApproveAchievementRequestAsync()
        {
            var expectedRequestAchievement = RequestAchievementDumbData.GetRandomEntity();

            //Arrange
            _requestAchievementRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(expectedRequestAchievement));
            _userAchievementService.Setup(x => x.AddAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

            //Arrange for DeleteAsync Function
            _requestAchievementRepository.Setup(x => x.Delete(It.IsAny<RequestAchievement>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _requestAchievementService.ApproveAchievementRequestAsync(expectedRequestAchievement.Id);

            // Assert
            _requestAchievementRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _requestAchievementRepository.Verify(x => x.Delete(It.IsAny<RequestAchievement>()), Times.Once); 
            _userAchievementService.Verify(x => x.AddAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
        }
    }
}
