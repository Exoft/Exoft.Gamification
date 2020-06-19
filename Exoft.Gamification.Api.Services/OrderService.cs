﻿using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService
        (
            IOrderRepository orderRepository,
            ICategoryRepository categoryRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadOrderModel> AddOrderAsync(CreateOrderModel model)
        {
            var order = _mapper.Map<Order>(model);
            
            order.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, order.IconId);

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

            return _mapper.Map<ReadOrderModel>(order);
        }

        public async Task<ReadOrderModel> UpdateOrderAsync(UpdateOrderModel model, Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            order.Title = model.Title;
            order.Description = model.Description;
            order.Price = model.Price;
            order.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, order.IconId);

            var categoryIds = order.Categories.Select(i => i.Category.Id).ToList();
            foreach (var categoryId in categoryIds)
            {
                if (!model.CategoryIds.Contains(categoryId))
                {
                    var orderCategory = order.Categories.First(i => i.Category.Id == categoryId);

                    order.Categories.Remove(orderCategory);
                }
            }

            foreach (var categoryId in model.CategoryIds)
            {
                if (!categoryIds.Contains(categoryId))
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    var orderCategory = new OrderCategory
                    {
                        Category = category,
                        Order = order
                    };

                    order.Categories.Add(orderCategory);
                }
            }

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadOrderModel>(order);
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

            return _mapper.Map<ReadOrderModel>(order);
        }

        public async Task<ReturnPagingInfo<ReadOrderModel>> GetAllOrderAsync(PagingInfo pagingInfo)
        {
            var page = await _orderRepository.GetAllDataAsync(pagingInfo);

            var readOrderModels = page.Data.Select(order => _mapper.Map<ReadOrderModel>(order)).ToList();
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
