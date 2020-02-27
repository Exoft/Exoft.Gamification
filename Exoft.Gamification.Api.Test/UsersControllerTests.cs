using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Controllers;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.TestData;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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
        public async Task GetUsersShortInfoAsync_Valid(PagingInfo pagingInfo)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var listUsers = DumbData.GetListReadShortUserModel(usersCount);
            var usersPagingInfo = DumbData.GetReturnPagingInfoForModel(pagingInfo, listUsers);

            _userService.Setup(x => x.GetAllUsersWithShortInfoAsync(pagingInfo)).Returns(Task.FromResult(usersPagingInfo));

            // Act
            var response = await controller.GetUsersShortInfoAsync(pagingInfo);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual(200, responseResult.StatusCode);
            Assert.IsNotNull(responseResult?.Value);

            Assert.IsTrue(responseResult?.Value is ReturnPagingInfo<ReadShortUserModel>);

            var responseValue = responseResult?.Value as ReturnPagingInfo<ReadShortUserModel>;
            Assert.AreEqual(usersPagingInfo, responseValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.InvalidPagingInfos))]
        public async Task GetUsersShortInfoAsync_Invalid(PagingInfo pagingInfo)
        {
            // Arrange
            var controller = new UsersController(_userService.Object, _createUserModelValidator.Object, _updateFullUserModelValidator.Object);

            var listUsers = DumbData.GetListReadShortUserModel(usersCount);
            var usersPagingInfo = DumbData.GetReturnPagingInfoForModel(pagingInfo, listUsers);

            _userService.Setup(x => x.GetAllUsersWithShortInfoAsync(pagingInfo)).Returns(Task.FromResult(usersPagingInfo));

            // Act
            var response = await controller.GetUsersShortInfoAsync(pagingInfo);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual(200, responseResult.StatusCode);
            Assert.IsNotNull(responseResult?.Value);

            Assert.IsTrue(responseResult?.Value is ReturnPagingInfo<ReadShortUserModel>);

            var responseValue = responseResult?.Value as ReturnPagingInfo<ReadShortUserModel>;
            Assert.AreEqual(usersPagingInfo, responseValue);
        }
    }
}