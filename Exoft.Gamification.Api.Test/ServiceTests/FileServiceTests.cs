using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
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
    public class FileServiceTests
    {
        private Mock<IFileRepository> _fileRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private IMapper _mapper;

        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _fileRepository = new Mock<IFileRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _fileService = new FileService(_fileRepository.Object, _unitOfWork.Object, _mapper);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidSingleGuid))]
        public async Task GetFileByIdAsync_ValidGuid_ReturnsFileModel(Guid id)
        {
            //Arrange
            var file = FileDumbData.GetEntity();
            id = file.Id;
            var expectedValue = _mapper.Map<FileModel>(file);
            var cancellationToken = new CancellationToken();

            _fileRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(file));

            //Act
            var response = await _fileService.GetFileByIdAsync(id, cancellationToken);

            //Assert
            _fileRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidIFormFileWithNullableGuid))]
        public async Task AddOrUpdateFileByIdAsync_ValidIFormFileAndGuid_ReturnsGuid(IFormFile image, Guid? id)
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>(), cancellationToken)).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>(), cancellationToken)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken));

            //Act
            var response = await _fileService.AddOrUpdateFileByIdAsync(image, id, cancellationToken);

            //Assert
            if (image != null)
            {
                if (id.HasValue)
                {
                    _fileRepository.Verify(x => x.Delete(It.IsAny<Guid>(), cancellationToken), Times.Once);
                }
                _fileRepository.Verify(x => x.AddAsync(It.IsAny<File>(), cancellationToken), Times.Once);
                _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
                response.Should().HaveValue();
                response.Should().NotBe(id.GetValueOrDefault());
            }
            else
            {
                response.Should().Be(id);
            }
        }
    }
}
