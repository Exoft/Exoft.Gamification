using Exoft.Gamification.Api.Common.Models.ReferenceBook;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Test
{

    [TestFixture]
    public class ReferenceBookServiceTests
    {
        private Mock<IChapterRepository> _chapterRepository;
        private Mock<IArticleRepository> _articleRepository;
        private Mock<IUnitOfWork> _unitOfWork;

        private IReferenceBookService _referenceBookService;

        [SetUp]
        public void SetUp()
        {
            _chapterRepository = new Mock<IChapterRepository>();
            _articleRepository = new Mock<IArticleRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _referenceBookService = new ReferenceBookService(_chapterRepository.Object, _articleRepository.Object,
                _unitOfWork.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.PagingInfos))]
        public async Task GetAllAchievementsByUserAsync(PagingInfo pagingInfo)
        {
            //Arrange
            var chapters = ChapterDumbData.GetRandomEntities(5);
            var returnPage = ReturnPagingInfoDumbData.GetForModel(pagingInfo, chapters);

            _chapterRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>())).ReturnsAsync(returnPage);

            // Act
            var response = await _referenceBookService.GetAllChaptersAsync(pagingInfo);

            // Assert
            _chapterRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>()), Times.Once);

            response.Should().BeEquivalentTo(returnPage);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateArticleModels))]
        public async Task UpdateArticleAsync_ValidModel(UpdateArticleModel updatedArticle)
        {
            var article = ArticleDumbData.GetRandomEntity();
            //Arrange
            _articleRepository.Setup(x => x.GetByIdAsync(updatedArticle.Id)).ReturnsAsync(article);
            _articleRepository.Setup(x => x.Update(It.IsAny<Article>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _referenceBookService.UpdateArticleAsync(updatedArticle);

            // Assert
            _articleRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _articleRepository.Verify(x => x.Update(It.IsAny<Article>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);

            response.Should().BeTrue();
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidUpdateArticleModels))]
        public async Task UpdateArticleAsync_InvalidModel(UpdateArticleModel updatedArticle)
        {
            Article article = null;
            //Arrange
            _articleRepository.Setup(x => x.GetByIdAsync(updatedArticle.Id)).ReturnsAsync(article);
            _articleRepository.Setup(x => x.Update(It.IsAny<Article>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _referenceBookService.UpdateArticleAsync(updatedArticle);

            // Assert
            _articleRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _articleRepository.Verify(x => x.Update(It.IsAny<Article>()), Times.Never);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);

            response.Should().BeFalse();
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateChapterModels))]
        public async Task AppChapterAsync_ValidModel(CreateChapterModel chapterModel)
        {
            //Arrange
            _chapterRepository.Setup(x => x.GetMaxOrderId()).Returns(RandomHelper.GetRandomNumber());
            _chapterRepository.Setup(x => x.AddAsync(It.IsAny<Chapter>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _referenceBookService.AddChapterAsync(chapterModel);

            // Assert
            _chapterRepository.Verify(x => x.GetMaxOrderId(), Times.Once);
            _chapterRepository.Verify(x => x.AddAsync(It.IsAny<Chapter>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);

            response.Should().BeTrue();
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateArticleModels))]
        public async Task AppArticleAsync_ValidModel(CreateArticleModel chapterModel)
        {
            //Arrange
            _chapterRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ChapterDumbData.GetRandomEntity());
            _chapterRepository.Setup(x => x.Update(It.IsAny<Chapter>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var response = await _referenceBookService.AddArticleAsync(chapterModel);

            // Assert
            _chapterRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _chapterRepository.Verify(x => x.Update(It.IsAny<Chapter>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);

            response.Should().BeTrue();
        }

        [TestCase]
        public async Task DeleteChapterAsync()
        {
            var chapter = ChapterDumbData.GetRandomEntity();
            //Arrange
            _chapterRepository.Setup(x => x.Delete(It.IsAny<Chapter>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _referenceBookService.DeleteChapterAsync(chapter);

            // Assert
            _chapterRepository.Verify(x => x.Delete(It.IsAny<Chapter>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase]
        public async Task DeleteArticleAsync()
        {
            var article = ArticleDumbData.GetRandomEntity();
            //Arrange
            _articleRepository.Setup(x => x.Delete(It.IsAny<Article>()));
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _referenceBookService.DeleteArticleAsync(article);

            // Assert
            _articleRepository.Verify(x => x.Delete(It.IsAny<Article>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase]
        public async Task GetChapterById()
        {
            var chapter = ChapterDumbData.GetRandomEntity();
            //Arrange
            _chapterRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(chapter);

            // Act
            var response = await _referenceBookService.GetChapterById(chapter.Id);

            // Assert
            _chapterRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            response.Should().BeEquivalentTo(chapter);
        }

        [TestCase]
        public async Task GetArticleById()
        {
            var article = ArticleDumbData.GetRandomEntity();
            //Arrange
            _articleRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(article);

            // Act
            var response = await _referenceBookService.GetArticleById(article.Id);

            // Assert
            _articleRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            response.Should().BeEquivalentTo(article);
        }

    }
}
