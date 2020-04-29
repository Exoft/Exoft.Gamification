using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IResponse> AddAsync(CreateRequestOrderModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var order = await _orderRepository.GetByIdAsync(model.OrderId);

            if (user.XP < order.Price)
            {
                return new NotAllowedResponse("The user doesn't have enough XP!");
            }

            var requestOrderPage = _stringLocalizer["RequestOrderPage"].ToString();
            var pageWithParams = requestOrderPage.Replace("{userLastName}", user.LastName);
            pageWithParams = pageWithParams.Replace("{userFirstName}", user.FirstName);
            pageWithParams = pageWithParams.Replace("{message}", model.Message);
            pageWithParams = pageWithParams.Replace("{orderTitle}", order.Title);

            var emails = await _userRepository.GetAdminsEmailsAsync();

            await _emailService.SendEmailsAsync("Request order", pageWithParams, emails.ToArray());

            var entity = _mapper.Map<RequestOrder>(model);
            entity.UserId = userId;
            entity.Status = GamificationEnums.RequestStatus.Pending;

            user.XP -= order.Price;

            _userRepository.Update(user);

            await _requestOrderRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new OkResponse();
        }

        public async Task<ReadRequestOrderModel> GetByIdAsync(Guid id)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id);

            return _mapper.Map<ReadRequestOrderModel>(requestOrder);
        }

        public async Task<IEnumerable<ReadRequestOrderModel>> GetAllAsync()
        {
            var pagingInfo = new PagingInfo
            {
                CurrentPage = 1,
                PageSize = 0
            };

            var readRequestOrderModels = new List<ReadRequestOrderModel>();

            var requestOrders = await _requestOrderRepository.GetAllDataAsync(pagingInfo);
            foreach (var item in requestOrders.Data)
            {
                var user = await _userRepository.GetByIdAsync(item.UserId);
                var order = await _orderRepository.GetByIdAsync(item.OrderId);
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

        public async Task ApproveOrderRequestAsync(Guid id)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id);

            requestOrder.Status = GamificationEnums.RequestStatus.Approved;

            _requestOrderRepository.Update(requestOrder);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeclineOrderRequestAsync(Guid id)
        {
            var requestOrder = await _requestOrderRepository.GetByIdAsync(id);

            var order = await _orderRepository.GetByIdAsync(requestOrder.OrderId);
            var user = await _userRepository.GetByIdAsync(requestOrder.UserId);

            user.XP += order.Price;

            _userRepository.Update(user);

            requestOrder.Status = GamificationEnums.RequestStatus.Declined;

            _requestOrderRepository.Update(requestOrder);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
