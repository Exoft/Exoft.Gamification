using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;
using Exoft.Gamification.Api.Test.TestData;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using System;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Test.DumbData;

namespace Exoft.Gamification.Api.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IFileRepository> _fileRepository;
        private Mock<IRoleRepository> _roleRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPasswordHasher> _hasher;
        private Mock<IStringLocalizer<HtmlPages>> _stringLocalizer;
        private Mock<IEmailService> _emailService;
        private Mock<IUserAchievementRepository> _userAchievementRepository;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _fileRepository = new Mock<IFileRepository>();
            _roleRepository = new Mock<IRoleRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _unitOfWork = new Mock<IUnitOfWork>();
            _hasher = new Mock<IPasswordHasher>();

            _stringLocalizer = new Mock<IStringLocalizer<HtmlPages>>();
            _emailService = new Mock<IEmailService>();
            _userAchievementRepository = new Mock<IUserAchievementRepository>();
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateUserModels))]
        public async Task AddUserAsync_ValidCreateUserModels_ReturnsReadFullUserInfo(CreateUserModel model)
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var expectedValue = UserDumbData.GetReadFullUserModel(model);
            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _emailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            string key = "RegisterPage";
            var localizedString = new LocalizedString(key, key);
            _stringLocalizer.Setup(_ => _[key]).Returns(localizedString);
            _hasher.Setup(x => x.GetHash(It.IsAny<string>())).Returns(model.Password);
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _roleRepository.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>())).Returns((string x) => Task.FromResult(RoleDumbData.GetEntity(x)));

            // Act
            var response = await userService.AddUserAsync(model);
            expectedValue.Id = response.Id;

            // Assert
            _userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _emailService.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _stringLocalizer.Verify(_ => _[key], Times.Once);
            _hasher.Verify(x => x.GetHash(It.IsAny<string>()), Times.Once);
            //_fileRepository.Verify(x => x.AddAsync(It.IsAny<File>()), Times.Once);
            //_fileRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
            _roleRepository.Verify(x => x.GetRoleByNameAsync(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(response);
            expectedValue.Should().BeEquivalentTo(response);
        }

        [TestCase]
        public async Task DeleteUserAsync_ValidGuids()
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var user = UserDumbData.GetRandomEntity();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _userRepository.Setup(x => x.Delete(It.IsAny<User>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await userService.DeleteUserAsync(user.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdatePasswordAsyncModels))]
        public async Task UpdatePasswordAsync_ValidUpdatePasswordAsyncModels(string newPassword)
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var user = UserDumbData.GetRandomEntity();

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            _userRepository.Setup(x => x.Update(It.IsAny<User>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _hasher.Setup(x => x.GetHash(It.IsAny<string>())).Returns(newPassword);

            // Act
            await userService.UpdatePasswordAsync(user.Id, newPassword);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _hasher.Verify(x => x.GetHash(It.IsAny<string>()), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.PagingInfos))]
        public async Task GetAllUsersWithShortInfoAsync_ValidPagingInfos_ReturnsReturnPagingInfo_ReadShortUserModel(PagingInfo pagingInfo)
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var listUsers = UserDumbData.GetRandomEntities(5);
            var expectedUsers = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listUsers);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadShortUserModel, User>(expectedUsers, _mapper);

            _userRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).Returns(Task.FromResult(expectedUsers));

            // Act
            var response = await userService.GetAllUsersWithShortInfoAsync(pagingInfo);

            // Assert
            _userRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.PagingInfos))]
        public async Task GetAllUsersWithFullInfoAsync_ValidPagingInfos_ReturnsReturnPagingInfo_ReadFullUserModel(PagingInfo pagingInfo)
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var listUsers = UserDumbData.GetRandomEntities(5);
            var expectedUsers = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listUsers);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadFullUserModel, User>(expectedUsers, _mapper);

            _userRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).Returns(Task.FromResult(expectedUsers));

            // Act
            var response = await userService.GetAllUsersWithFullInfoAsync(pagingInfo);

            // Assert
            _userRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task GetFullUserByIdAsync_ValidGuids_ReturnsReadFullUserModel()
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var expectedUser = UserDumbData.GetRandomEntity();
            var badgetCount = RandomHelper.GetRandomNumber();
            var expectedValue = _mapper.Map<ReadFullUserModel>(expectedUser);
            expectedValue.BadgesCount = badgetCount;

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(expectedUser));
            _userAchievementRepository.Setup(x => x.GetCountAchievementsByUserAsync(It.IsAny<Guid>())).ReturnsAsync(badgetCount);

            // Act
            var response = await userService.GetFullUserByIdAsync(expectedUser.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task GetShortUserByIdAsync_ValidGuids_ReturnsReadShortUserModel()
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var expectedUser = UserDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadShortUserModel>(expectedUser);

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(expectedUser));

            // Act
            var response = await userService.GetShortUserByIdAsync(expectedUser.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateFullUserModels))]
        public async Task UpdateUserAsync_ValidUpdateFullUserModelsWithIds_ReturnsReadFullUserModel(UpdateUserModel model)
        {
            //Arrange
            var userService = new UserService(_userRepository.Object, _fileRepository.Object, _roleRepository.Object,
                _mapper, _unitOfWork.Object, _hasher.Object, _stringLocalizer.Object,
                _emailService.Object, _userAchievementRepository.Object);

            var expectedUser = UserDumbData.GetEntity(model);
            var expectedValue = _mapper.Map<ReadFullUserModel>(expectedUser);

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(expectedUser));
            _userRepository.Setup(x => x.Update(It.IsAny<User>()));
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _roleRepository.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>())).Returns((string x) => Task.FromResult(RoleDumbData.GetEntity(x))).Verifiable();
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await userService.UpdateUserAsync(model, expectedUser.Id);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            //_fileRepository.Verify(x => x.AddAsync(It.IsAny<File>()), Times.Once);
            //_fileRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            _roleRepository.Verify(x => x.GetRoleByNameAsync(It.IsAny<string>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}