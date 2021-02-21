using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService
        (
            IEventRepository eventRepository,
            IMapper mapper
        )
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<ReturnPagingInfo<EventModel>> GetAllEventAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
        {
            var page = await _eventRepository.GetAllDataAsync(pagingInfo, cancellationToken);

            var eventModels = page.Data.Select(i => _mapper.Map<EventModel>(i)).ToList();
            var result = new ReturnPagingInfo<EventModel>
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = eventModels
            };

            return result;
        }
    }
}
