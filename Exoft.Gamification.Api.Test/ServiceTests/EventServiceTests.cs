using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Test.DumbData;
using Exoft.Gamification.Api.Test.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exoft.Gamification.Api.Test.ServiceTests
{
    [TestFixture]
    public class EventServiceTests
    {
        private Mock<IEventRepository> _eventRepository;
        private IMapper _mapper;

        private IEventService _eventService;

        [SetUp]
        public void SetUp()
        {
            _eventRepository = new Mock<IEventRepository>();

            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _eventService = new EventService(_eventRepository.Object, _mapper);
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.ValidPagingInfos))]
        public async Task GetAllEventAsync_PagingInfo_ReturnsReturnPagingInfo_EventModel(PagingInfo pagingInfo)
        {
            //Arrange
            var listOfEvents = EventDumbData.GetRandomEntities(5); 
            var paging = ReturnPagingInfoDumbData.GetForModel(pagingInfo, listOfEvents);
            var expectedValue = ReturnPagingInfoDumbData.GetWithModels<EventModel, Event>(paging, _mapper);
            var cancellationToken = new CancellationToken();

            _eventRepository.Setup(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken)).Returns(Task.FromResult(paging));

            // Act
            var response = await _eventService.GetAllEventAsync(pagingInfo, cancellationToken);

            // Assert
            _eventRepository.Verify(x => x.GetAllDataAsync(It.IsAny<PagingInfo>(), cancellationToken), Times.Once);
            response.Should().BeEquivalentTo(expectedValue);
        }
    }
}
