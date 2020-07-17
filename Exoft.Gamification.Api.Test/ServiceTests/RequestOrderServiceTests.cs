using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.RequestOrder;
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
    public class RequestOrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IRequestOrderRepository> _requestOrderRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStringLocalizer<HtmlPages>> _stringLocalizer;
        private Mock<IEmailService> _emailService;
        private IMapper _mapper;

        private IRequestOrderService _requestOrderService;

        [SetUp]
        public void SetUp()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _userRepository = new Mock<IUserRepository>();
            _requestOrderRepository = new Mock<IRequestOrderRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _stringLocalizer = new Mock<IStringLocalizer<HtmlPages>>();
            _emailService = new Mock<IEmailService>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _requestOrderService = new RequestOrderService(
                _requestOrderRepository.Object, _userRepository.Object,
                _orderRepository.Object,
                _mapper, _unitOfWork.Object, 
                _stringLocalizer.Object, _emailService.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateRequestOrderModels))]
        public async Task AddAsync_CreateRequestOrderModel_ReturnsOkResponse(CreateRequestOrderModel model, Guid userId)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            userId = user.Id;
            var order = OrderDumbData.GetRandomEntity();
            model.OrderId = order.Id;
            var enoughXP = user.XP < order.Price;

            ICollection<string> emailCollection = new List<string> {
                    RandomHelper.GetRandomString(),
                    RandomHelper.GetRandomString(),
                };

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(order));
            _userRepository.Setup(x => x.GetAdminsEmailsAsync()).Returns(Task.FromResult(emailCollection));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _emailService.Setup(x => x.SendEmailsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>())).Returns(Task.CompletedTask);
            string key = "RequestOrderPage";
            var localizedString = new LocalizedString(key, key);
            _stringLocalizer.Setup(_ => _[key]).Returns(localizedString);

            // Act
            var response = await _requestOrderService.AddAsync(model, userId);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            if (enoughXP)
            {
                response.Should().BeOfType<NotAllowedResponse>();
            }
            else
            {
                _userRepository.Verify(x => x.GetAdminsEmailsAsync(), Times.Once);
                _stringLocalizer.Verify(_ => _[key], Times.Once);
                _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
                _emailService.Verify(x => x.SendEmailsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
                response.Should().BeOfType<OkResponse>();
            }
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task GetByIdAsync_ValidRequestOrderId(Guid id)
        {
            //Arrange
            var requestOrder = RequestOrderDumbData.GetRandomEntity();

            _requestOrderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(requestOrder);

            // Act
            await _requestOrderService.GetByIdAsync(id);

            // Assert
            _requestOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [TestCase]
        public async Task GetAllAsync_ValidOrderId()
        {
            //Arrange
            var users = UserDumbData.GetRandomEntities(5);
            var orders = OrderDumbData.GetRandomEntities(users.Count);
            var requestOrders = RequestOrderDumbData.GetEntities(users, orders);

            var expectedRequestOrders = ReturnPagingInfoDumbData.GetForModel(new PagingInfo(), requestOrders);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadRequestOrderModel, RequestOrder>(expectedRequestOrders, _mapper);

            _requestOrderRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).Returns(Task.FromResult(expectedRequestOrders));
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns((Guid x) => Task.FromResult(users.FirstOrDefault(y => y.Id == x)));
            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns((Guid x) => Task.FromResult(orders.FirstOrDefault(y => y.Id == x)));


            // Act
            var response = await _requestOrderService.GetAllAsync();

            // Assert
            _requestOrderRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(users.Count));
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(orders.Count));
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task ApproveOrderRequestAsync_ValidGuid(Guid id)
        {
            //Arrange
            var requestOrder = RequestOrderDumbData.GetRandomEntity();
            id = requestOrder.Id;

            _requestOrderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(requestOrder));
            _requestOrderRepository.Setup(x => x.Update(It.IsAny<RequestOrder>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _requestOrderService.ApproveOrderRequestAsync(id);

            // Assert
            _requestOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _requestOrderRepository.Verify(x => x.Update(It.IsAny<RequestOrder>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task DeclineOrderRequestAsync_ValidGuid(Guid id)
        {
            //Arrange
            var user = UserDumbData.GetRandomEntity();
            var order = OrderDumbData.GetRandomEntity();
            var requestOrder = RequestOrderDumbData.GetEntity(user.Id, order.Id);
            id = requestOrder.Id;

            _requestOrderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(requestOrder));
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(order));
            _userRepository.Setup(x => x.Update(It.IsAny<User>()));
            _requestOrderRepository.Setup(x => x.Update(It.IsAny<RequestOrder>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _requestOrderService.DeclineOrderRequestAsync(id);

            // Assert
            _requestOrderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _userRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            _requestOrderRepository.Verify(x => x.Update(It.IsAny<RequestOrder>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
