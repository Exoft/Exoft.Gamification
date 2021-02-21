using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;

namespace Exoft.Gamification.Api.Test.ServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IFileService> _fileService;
        private Mock<IRoleRepository> _roleRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPasswordHasher> _hasher;
        private Mock<IStringLocalizer<HtmlPages>> _stringLocalizer;
        private Mock<IEmailService> _emailService;
        private Mock<IUserAchievementRepository> _userAchievementRepository;

        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _fileService = new Mock<IFileService>();
            _roleRepository = new Mock<IRoleRepository>();
            _userAchievementRepository = new Mock<IUserAchievementRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();
            _hasher = new Mock<IPasswordHasher>();

            _stringLocalizer = new Mock<IStringLocalizer<HtmlPages>>();
            _emailService = new Mock<IEmailService>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _userService = new UserService(_userRepository.Object, _fileService.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateUserModels))]
        public async Task AddUserAsync_ValidCreateUserModels_ReturnsReadFullUserInfo(CreateUserModel model)
        {
            //Arrange            
            var cancellationToken = new CancellationToken();
            var expectedValue = UserDumbData.GetReadFullUserModel(model);
            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), cancellationToken)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);
            _emailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cancellationToken)).Returns(Task.CompletedTask);

            string key = "RegisterPage";
            var localizedString = new LocalizedString(key, key);
            _stringLocalizer.Setup(_ => _[key]).Returns(localizedString);
            _hasher.Setup(x => x.GetHash(It.IsAny<string>())).Returns(model.Password);
            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult(expectedValue.AvatarId));
            _roleRepository.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>(), cancellationToken)).Returns((string x) => Task.FromResult(RoleDumbData.GetEntity(x)));

            // Act
            var response = await _userService.AddUserAsync(model, cancellationToken);
            expectedValue.Id = response.Id;

            // Assert
            _userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            _emailService.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Once);
            _stringLocalizer.Verify(_ => _[key], Times.Once);
            _hasher.Verify(x => x.GetHash(It.IsAny<string>()), Times.Once);
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _roleRepository.Verify(x => x.GetRoleByNameAsync(It.IsAny<string>(), cancellationToken), Times.Once);
            Assert.IsNotNull(response);
            expectedValue.Should().BeEquivalentTo(response);
        }

        [TestCase]
        public async Task DeleteUserAsync_ValidGuids()
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).ReturnsAsync(user);
            _userRepository.Setup(x => x.Delete(It.IsAny<User>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(user.Id, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _userRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdatePasswordAsyncModels))]
        public async Task UpdatePasswordAsync_ValidUpdatePasswordAsyncModels(string newPassword)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(user));
            _userRepository.Setup(x => x.Update(It.IsAny<User>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);
            _hasher.Setup(x => x.GetHash(It.IsAny<string>())).Returns(newPassword);

            // Act
            await _userService.UpdatePasswordAsync(user.Id, newPassword, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            _hasher.Verify(x => x.GetHash(It.IsAny<string>()), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllUsersWithShortInfoAsync_ValidPagingInfos_ReturnsReturnPagingInfo_ReadShortUserModel(PagingInfo pagingInfo)
        {
            //Arrange
            var listUsers = UserDumbData.GetRandomEntities(5);
            var cancellationToken = new CancellationToken();
            var expectedUsers = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listUsers);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadShortUserModel, User>(expectedUsers, _mapper);

            _userRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(expectedUsers));

            // Act
            var response = await _userService.GetAllUsersWithShortInfoAsync(pagingInfo, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllUsersWithFullInfoAsync_ValidPagingInfos_ReturnsReturnPagingInfo_ReadFullUserModel(PagingInfo pagingInfo)
        {
            //Arrange
            var listUsers = UserDumbData.GetRandomEntities(5);
            var cancellationToken = new CancellationToken();
            var expectedUsers = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listUsers);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadFullUserModel, User>(expectedUsers, _mapper);

            _userRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(expectedUsers));

            // Act
            var response = await _userService.GetAllUsersWithFullInfoAsync(pagingInfo, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task GetFullUserByIdAsync_ValidGuids_ReturnsReadFullUserModel()
        {
            //Arrange
            var expectedUser = UserDumbData.GetRandomEntity();
            var badgetCount = RandomHelper.GetRandomNumber();
            var expectedValue = _mapper.Map<ReadFullUserModel>(expectedUser);
            expectedValue.BadgesCount = badgetCount;
            var cancellationToken = new CancellationToken();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(expectedUser));
            _userAchievementRepository.Setup(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>(), cancellationToken)).ReturnsAsync(badgetCount);

            // Act
            var response = await _userService.GetFullUserByIdAsync(expectedUser.Id, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task GetShortUserByIdAsync_ValidGuids_ReturnsReadShortUserModel()
        {
            //Arrange
            var expectedUser = UserDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadShortUserModel>(expectedUser);
            var cancellationToken = new CancellationToken();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(expectedUser));

            // Act
            var response = await _userService.GetShortUserByIdAsync(expectedUser.Id, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateFullUserModels))]
        public async Task UpdateUserAsync_ValidUpdateFullUserModelsWithIds_ReturnsReadFullUserModel(UpdateUserModel model)
        {
            //Arrange
            var expectedUser = UserDumbData.GetEntity(model);
            var expectedValue = _mapper.Map<ReadFullUserModel>(expectedUser);
            var cancellationToken = new CancellationToken();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(expectedUser));
            _userRepository.Setup(x => x.Update(It.IsAny<User>()));
            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult(expectedValue.AvatarId));
            _roleRepository.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>(), cancellationToken)).Returns((string x) => Task.FromResult(RoleDumbData.GetEntity(x))).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _userService.UpdateUserAsync(model, expectedUser.Id, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            _roleRepository.Verify(x => x.GetRoleByNameAsync(It.IsAny<string>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}