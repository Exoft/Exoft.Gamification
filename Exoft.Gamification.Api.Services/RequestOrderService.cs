using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.RequestOrder;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;

using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Services
{
    public class RequestOrderService : IRequestOrderService
    {
        private readonly IRequestOrderRepository _requestOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<HtmlPages> _stringLocalizer;
        private readonly IEmailService _emailService;

        public RequestOrderService
        (
            IRequestOrderRepository requestOrderRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStringLocalizer<HtmlPages> stringLocalizer,
            IEmailService emailService
        )
        {
            _requestOrderRepository = requestOrderRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
        }

        public async Task<IResponse> AddAsync(CreateRequestOrderModel model, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            var order = await _orderRepository.GetByIdAsync(model.OrderId, cancellationToken);

            if (user.XP < order.Price)
            {
                return new NotAllowedResponse("The user doesn't have enough XP!");
            }

            var requestOrderPage = _stringLocalizer["RequestOrderPage"].ToString();
            var pageWithParams = requestOrderPage.Replace("{userLastName}", user.LastName);
            pageWithParams = pageWithParams.Replace("{userFirstName}", user.FirstName);
            pageWithParams = pageWithParams.Replace("{message}", model.Message);
            pageWithParams = pageWithParams.Replace("{orderTitle}", order.Title);

            var emails = await _userRepository.GetAdminsEmailsAsync(cancellationToken);

            await _emailService.SendEmailsAsync("Request order", pageWithParams, cancellationToken, emails.ToArray());

            var entity = _mapper.Map<RequestOrder>(model);
            entity.UserId = userId;
            entity.Status = GamificationEnums.RequestStatus.Pending;

            user.XP -= order.Price;

            _userRepository.Update(user);

            await _requestOrderRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OkResponse();
        }

        public async Task<ReadRequestOrderModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<ReadRequestOrderModel>(requestOrder);
        }

        public async Task<IEnumerable<ReadRequestOrderModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var pagingInfo = new PagingInfo
            {
                CurrentPage = 1,
                PageSize = 0
            };

            var readRequestOrderModels = new List<ReadRequestOrderModel>();

            var requestOrders = await _requestOrderRepository.GetAllDataAsync(pagingInfo, cancellationToken);
            foreach (var item in requestOrders.Data)
            {
                var user = await _userRepository.GetByIdAsync(item.UserId, cancellationToken);
                var order = await _orderRepository.GetByIdAsync(item.OrderId, cancellationToken);
                readRequestOrderModels.Add(new ReadRequestOrderModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    UserId = item.UserId,
                    OrderName = order.Title,
                    UserName = user.UserName,
                    Message = item.Message
                });
            }

            return readRequestOrderModels;
        }

        public async Task ApproveOrderRequestAsync(Guid id, CancellationToken cancellationToken)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id, cancellationToken);

            requestOrder.Status = GamificationEnums.RequestStatus.Approved;

            _requestOrderRepository.Update(requestOrder);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeclineOrderRequestAsync(Guid id, CancellationToken cancellationToken)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id, cancellationToken);

            var order = await _orderRepository.GetByIdAsync(requestOrder.OrderId, cancellationToken);
            var user = await _userRepository.GetByIdAsync(requestOrder.UserId, cancellationToken);

            user.XP += order.Price;

            _userRepository.Update(user);

            requestOrder.Status = GamificationEnums.RequestStatus.Declined;

            _requestOrderRepository.Update(requestOrder);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
