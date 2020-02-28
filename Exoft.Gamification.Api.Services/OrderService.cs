using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService
        (
            IOrderRepository orderRepository,
            ICategoryRepository categoryRepository,
            IFileRepository fileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadOrderModel> AddOrderAsync(CreateOrderModel model)
        {
            var order = _mapper.Map<Order>(model);

            if (model.Icon != null)
            {
                using (var memory = new MemoryStream())
                {
                    await model.Icon.CopyToAsync(memory);

                    var file = new File
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Icon.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    order.IconId = file.Id;
                }
            }

            foreach (var categoryId in model.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);

                var orderCategory = new OrderCategory
                {
                    Order = order,
                    Category = category
                };

                order.Categories.Add(orderCategory);
            }

            await _orderRepository.AddAsync(order);

            await _unitOfWork.SaveChangesAsync();

            var readOrderModel = _mapper.Map<ReadOrderModel>(order);
            foreach (var orderCategory in order.Categories)
            {
                readOrderModel.Categories.Add(_mapper.Map<ReadCategoryModel>(orderCategory.Category));
            }

            return readOrderModel;
        }

        public async Task<ReadOrderModel> UpdateOrderAsync(UpdateOrderModel model, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            _orderRepository.Delete(order);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReadOrderModel> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return null;
            }

            var readOrderModel = _mapper.Map<ReadOrderModel>(order);
            foreach (var orderCategory in order.Categories)
            {
                readOrderModel.Categories.Add(_mapper.Map<ReadCategoryModel>(orderCategory.Category));
            }

            return readOrderModel;
        }

        public async Task<ReturnPagingInfo<ReadOrderModel>> GetAllOrderAsync(PagingInfo pagingInfo)
        {
            var page = await _orderRepository.GetAllDataAsync(pagingInfo);

            var readOrderModels = page.Data.Select(order =>
            {
                var readOrderModel = _mapper.Map<ReadOrderModel>(order);
                foreach (var orderCategory in order.Categories)
                {
                    readOrderModel.Categories.Add(_mapper.Map<ReadCategoryModel>(orderCategory.Category));
                }

                return readOrderModel;
            }).ToList();

            var result = new ReturnPagingInfo<ReadOrderModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readOrderModels
            };

            return result;
        }
    }
}
