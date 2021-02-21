using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IOrderService
    {
        Task<ReadOrderModel> AddOrderAsync(CreateOrderModel model, CancellationToken cancellationToken);

        Task<ReadOrderModel> UpdateOrderAsync(UpdateOrderModel model, Guid id, CancellationToken cancellationToken);

        Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken);

        Task<ReadOrderModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<ReadOrderModel>> GetAllOrderAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);
    }
}
