using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Controllers;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.TestData;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Test
{
    [TestFixture]
    public class UsersControllerTests
    {
        private readonly int usersCount = 5;
        private Mock<IUserService> _userService;
        private Mock<IValidator<CreateUserModel>> _createUserModelValidator;
        private Mock<IValidator<UpdateFullUserModel>> _updateFullUserModelValidator;

        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();
            _createUserModelValidator = new Mock<IValidator<CreateUserModel>>();
            _updateFullUserModelValidator = new Mock<IValidator<UpdateFullUserModel>>();
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetUsersShortInfoAsync_ValidPagingInfo_ReturnsReturnPagingInfo(PagingInfo pagingInfo)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var listUsers = DumbData.GetRandomListReadShortUserModel(usersCount);
            var expectedValue = DumbData.GetReturnPagingInfoForModel(pagingInfo, listUsers);

            _userService.Setup(x => x.GetAllUsersWithShortInfoAsync(pagingInfo)).ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUsersShortInfoAsync(pagingInfo);

            // Assert
            Assert.IsNotNull(response);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReturnPagingInfo<ReadShortUserModel>;
            Assert.AreEqual(expectedValue, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.InvalidPagingInfos))]
        public async Task GetUsersShortInfoAsync_InvalidPagingInfo_ReturnsReturnPagingInfo(PagingInfo pagingInfo)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var listUsers = DumbData.GetRandomListReadShortUserModel(usersCount);
            var expectedValue = DumbData.GetReturnPagingInfoForModel(pagingInfo, listUsers);

            _userService.Setup(x => x.GetAllUsersWithShortInfoAsync(pagingInfo)).ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUsersShortInfoAsync(pagingInfo);

            // Assert
            Assert.IsNotNull(response);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReturnPagingInfo<ReadShortUserModel>;
            Assert.AreEqual(expectedValue, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetUsersFullInfoAsync_ValidPagingInfo_ReturnsReturnPagingInfo(PagingInfo pagingInfo)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var listUsers = DumbData.GetRandomListFullShortUserModel(usersCount);
            var expectedValue = DumbData.GetReturnPagingInfoForModel(pagingInfo, listUsers);

            _userService.Setup(x => x.GetAllUsersWithFullInfoAsync(pagingInfo)).ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUsersFullInfoAsync(pagingInfo);

            // Assert
            Assert.IsNotNull(response);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReturnPagingInfo<ReadFullUserModel>;
            Assert.AreEqual(expectedValue, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUserId))]
        public async Task GetUserByIdAsync_ValidUserId_ReturnsReadFullUserModel(Guid userId)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var expectedValue = DumbData.GetReadFullUserModelById(userId);

            _userService.Setup(x => x.GetFullUserByIdAsync(userId)).ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(response);
            var responseResult = response as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReadFullUserModel;
            Assert.AreEqual(expectedValue, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateUserModels))]
        public async Task AddUserAsync_ValidCreateUserModel_ReturnsReadFullUserModel(CreateUserModel user)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var expectedValue = DumbData.GetReadFullUserModel(user);

            _userService.Setup(x => x.AddUserAsync(user)).ReturnsAsync(expectedValue);
            _createUserModelValidator.Setup(x => x.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var response = await controller.AddUserAsync(user);

            // Assert
            Assert.IsNotNull(response);

            var responseResult = response as CreatedAtRouteResult;

            Assert.AreEqual((int)HttpStatusCode.Created, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReadFullUserModel;
            Assert.AreEqual(expectedValue, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateFullUserModelsWithIds))]
        public async Task UpdateUserAsync_ValidCreateUserModel_ReturnsReadFullUserModel(UpdateFullUserModel user, Guid userId)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var expectedValue = DumbData.GetReadFullUserModel(user, userId);

            _userService.Setup(x => x.GetFullUserByIdAsync(userId)).ReturnsAsync(expectedValue);
            _userService.Setup(x => x.UpdateUserAsync(user, userId)).ReturnsAsync(expectedValue);
            _updateFullUserModelValidator.Setup(x => x.ValidateAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Act
            var response = await controller.UpdateUserAsync(user, userId);

            // Assert
            Assert.IsNotNull(response);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, responseResult.StatusCode);
            Assert.IsNotNull(responseResult.Value);

            var responseValue = responseResult.Value as ReadFullUserModel;
            Assert.AreEqual(expectedValue, responseValue);
        }
    }
}