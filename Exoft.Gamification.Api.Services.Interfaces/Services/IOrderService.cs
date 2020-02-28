using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IOrderService
    {
        Task<ReadOrderModel> AddOrderAsync(CreateOrderModel model);

        Task<ReadOrderModel> UpdateOrderAsync(UpdateOrderModel model, Guid id);

        Task DeleteOrderAsync(Guid id);

        Task<ReadOrderModel> GetOrderByIdAsync(Guid id);

        Task<ReturnPagingInfo<ReadOrderModel>> GetAllOrderAsync(PagingInfo pagingInfo);
    }
}
