using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.Category;
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
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<IFileService> _fileService;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _fileService = new Mock<IFileService>();
            _unitOfWork = new Mock<IUnitOfWork>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _categoryService = new CategoryService(_categoryRepository.Object,
                _fileService.Object, _unitOfWork.Object, _mapper);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllCategoryAsync_PagingInfo_ReturnsReturnPagingInfo_ReadCategoryModel(PagingInfo pagingInfo)
        {
            //Arrange
            var listOfEvents = CategoryDumbData.GetRandomEntities(5);
            var paging = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listOfEvents);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<ReadCategoryModel, Category>(paging, _mapper);
            var cancellationToken = new CancellationToken();

            _categoryRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(paging));

            // Act
            var response = await _categoryService.GetAllCategoryAsync(pagingInfo, cancellationToken);

            // Assert
            _categoryRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateCategoryModels))]
        public async Task AddCategoryAsync_PagingInfo_ReturnsReturnPagingInfo_ReadCategoryModel(CreateCategoryModel model)
        {
            //Arrange
            var expectedValue = CategoryDumbData.GetReadCategoryModel(model);
            var cancellationToken = new CancellationToken();

            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult((Guid?)expectedValue.IconId));
            _categoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>(), cancellationToken)).Returns(Task.FromResult(expectedValue));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _categoryService.AddCategoryAsync(model, cancellationToken);
            expectedValue.Id = response.Id;

            // Assert
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _categoryRepository.Verify(x => x.AddAsync(It.IsAny<Category>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCase]
        public async Task DeleteCategoryAsync()
        {
            //Arrange
            var category = CategoryDumbData.GetRandomEntity();
            var cancellationToken = new CancellationToken();

            _categoryRepository.Setup(x => x.Delete(It.IsAny<Category>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteCategoryAsync(category.Id, cancellationToken);

            // Assert
            _categoryRepository.Verify(x => x.Delete(It.IsAny<Category>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [TestCase]
        public async Task GetCategoryByIdAsync()
        {
            //Arrange
            var category = CategoryDumbData.GetRandomEntity();
            var expectedValue = _mapper.Map<ReadCategoryModel>(category);
            var cancellationToken = new CancellationToken();

            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(category));

            // Act
            var response = await _categoryService.GetCategoryByIdAsync(category.Id, cancellationToken);

            // Assert
            _categoryRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateCategoryModels))]
        public async Task UpdateCategoryAsync_UpdateCategoryModel_ReturnsReadCategoryModel(UpdateCategoryModel model)
        {
            //Arrange
            var category = CategoryDumbData.GetEntity(model);
            var expectedValue = _mapper.Map<ReadCategoryModel>(category);
            var cancellationToken = new CancellationToken();

            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(category));
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>()));
            _fileService.Setup(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken)).Returns(Task.FromResult((Guid?)expectedValue.IconId));
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            var response = await _categoryService.UpdateCategoryAsync(model, category.Id, cancellationToken);

            // Assert
            _categoryRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _categoryRepository.Verify(x => x.Update(It.IsAny<Category>()), Times.Once);
            _fileService.Verify(x => x.AddOrUpdateFileByIdAsync(It.IsAny<IFormFile>(), It.IsAny<Guid?>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}
