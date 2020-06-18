using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Test
{
    [TestFixture]
    public class FileServiceTests
    {
        public Mock<IFileRepository> _fileRepository;
        private IMapper _mapper;

        private IFileService _fileService;

        [SetUp]
        public void SetUp()
        {
            _fileRepository = new Mock<IFileRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _fileService = new FileService(_fileRepository.Object, _mapper);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.SingleGuid))]
        public async Task GetFileByIdAsync_ValidGuid_ReturnsFileModel(Guid id)
        {
            //Arrange
            var file = FileDumbData.GetEntity();
            id = file.Id;
            var expectedValue = _mapper.Map<FileModel>(file);

            _fileRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(file));

            //Act
            var response = await _fileService.GetFileByIdAsync(id);

            //Assert
            _fileRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidIFormFileWithNullableGuid))]
        public async Task AddOrUpdateFileByIdAsync_ValidIFormFileAndGuid_ReturnsGuid(IFormFile image, Guid? id)
        {
            //Arrange
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);

            //Act
            var response = await _fileService.AddOrUpdateFileByIdAsync(image, id);

            //Assert
            _fileRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _fileRepository.Setup(x => x.AddAsync(It.IsAny<File>())).Returns(Task.CompletedTask);

            if (image != null)
            {
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
