using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Common.Models.Order;
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
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<IFileService> _fileService;
        private Mock<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;

        private IOrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _fileService = new Mock<IFileService>();
            _unitOfWork = new Mock<IUnitOfWork>();


            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _orderService = new OrderService(
                _orderRepository.Object, _categoryRepository.Object,
                _fileService.Object, _unitOfWork.Object,  _mapper);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateOrderModels))]
        public async Task AddOrderAsync_CreateOrderModel_ReturnsReadOrderModel(CreateOrderModel model)
        {
            //Arrange
            var expectedValue = OrderDumbData.GetReadAchievementModel(model);
            var categoriesList = CategoryDumbData.GetRandomEntities(2);
            model.CategoryIds = categoriesList.Select(x => x.Id);
            expectedValue.Categories = _mapper.Map<List<ReadCategoryModel>>(categoriesList);
            var cancellationToken = new CancellationToken();

            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult((Guid?)expectedValue.IconId));
            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns((Guid x) => Task.FromResult(categoriesList.FirstOrDefault(y => y.Id == x)));
            _orderRepository.Setup(x => x.AddAsync(It.IsAny<Order>(), cancellationToken)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _orderService.AddOrderAsync(model, cancellationToken);
            expectedValue.Id = response.Id;

            // Assert
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _categoryRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Exactly(2));
            _orderRepository.Verify(x => x.AddAsync(It.IsAny<Order>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateOrderModels))]
        public async Task UpdateOrderAsync_UpdateOrderModel_ReturnsReadOrderModel(UpdateOrderModel model)
        {
            //Arrange
            var order = OrderDumbData.GetEntity(model);
            var categoriesList = CategoryDumbData.GetRandomEntities(2);
            model.CategoryIds = categoriesList.Select(x => x.Id).ToList();
            var expectedValue = _mapper.Map<ReadOrderModel>(order);
            expectedValue.Categories = _mapper.Map<List<ReadCategoryModel>>(categoriesList);
            var cancellationToken = new CancellationToken();

            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(order));
            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult((Guid?)expectedValue.IconId));
            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns((Guid x) => Task.FromResult(categoriesList.FirstOrDefault(y => y.Id == x)));
            _orderRepository.Setup(x => x.Update(It.IsAny<Order>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _orderService.UpdateOrderAsync(model, order.Id, cancellationToken);

            // Assert
            _categoryRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Exactly(2));
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidSingleGuid))]
        public async Task DeleteOrderAsync_ValidOrderId(Guid orderId)
        {
            //Arrange
            var order = OrderDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).ReturnsAsync(order);
            _orderRepository.Setup(x => x.Delete(It.IsAny<Order>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _orderService.DeleteOrderAsync(orderId, cancellationToken);

            // Assert
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _orderRepository.Verify(x => x.Delete(It.IsAny<Order>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidSingleGuid))]
        public async Task GetOrderByIdAsync_ValidOrderId(Guid orderId)
        {
            //Arrange
            var order = OrderDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _orderRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).ReturnsAsync(order);

            // Act
            await _orderService.GetOrderByIdAsync(orderId, cancellationToken);

            // Assert
            _orderRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllOrderAsync_ValidOrderId(PagingInfo pagingInfo)
        {
            //Arrange
            var orders = OrderDumbData.GetRandomEntities(5);
            var cancellationToken = new CancellationToken();

            var expectedOrders = ReturnPagingInfoDumbData.GetForModel(pagingInfo, orders);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadOrderModel, Order>(expectedOrders, _mapper);

            _orderRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(expectedOrders));

            // Act
            var response = await _orderService.GetAllOrderAsync(pagingInfo, cancellationToken);

            // Assert
            _orderRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
        }
    }
}
