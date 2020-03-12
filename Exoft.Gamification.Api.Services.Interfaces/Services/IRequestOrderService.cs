using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestOrder;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRequestOrderService
    {
        Task<IResponse> AddAsync(CreateRequestOrderModel model, Guid userId);

        Task<ReadRequestOrderModel> GetByIdAsync(Guid id);

        Task<IEnumerable<ReadRequestOrderModel>> GetAllAsync();

        Task ApproveOrderRequestAsync(Guid id);

        Task DeclineOrderRequestAsync(Guid id);
    }
}
