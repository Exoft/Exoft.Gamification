using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
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
    }
}
