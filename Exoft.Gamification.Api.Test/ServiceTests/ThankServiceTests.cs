using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using Moq;
using NUnit.Framework;

namespace Exoft.Gamification.Api.Test.ServiceTests
{
    [TestFixture]
    public class ThankServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IThankRepository> _thankRepository;
        private IMapper _mapper;
        private Mock<IUnitOfWork> _unitOfWork;

        private IThankService _thankService;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _thankRepository = new Mock<IThankRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _unitOfWork = new Mock<IUnitOfWork>();

            _thankService = new ThankService(_userRepository.Object, _thankRepository.Object,
                _mapper, _unitOfWork.Object);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidCreateThankModels))]
        public async Task AddAsync_ValidCreateThankModel(CreateThankModel model)
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            
            var user = UserDumbData.GetRandomEntity();
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(user));
            _thankRepository.Setup(x => x.AddAsync(It.IsAny<Thank>(), cancellationToken)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _thankService.AddAsync(model, user.Id, cancellationToken);

            // Assert
            _userRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
            _thankRepository.Verify(x => x.AddAsync(It.IsAny<Thank>(), cancellationToken), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [TestCase]
        public async Task GetLastThankAsync()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var user = UserDumbData.GetRandomEntity();
            _thankRepository.Setup(x => x.GetLastThankAsync(It.IsAny<Guid>(), cancellationToken)).Returns(Task.FromResult(ThankDumbData.GetRandomEntity()));

            // Act
            await _thankService.GetLastThankAsync(user.Id, cancellationToken);

            // Assert
            _thankRepository.Verify(x => x.GetLastThankAsync(It.IsAny<Guid>(), cancellationToken), Times.Once);
        }
    }
}
