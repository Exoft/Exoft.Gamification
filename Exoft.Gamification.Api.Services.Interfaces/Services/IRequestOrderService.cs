using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestOrder;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRequestOrderService
    {
        Task<IResponse> AddAsync(CreateRequestOrderModel model, Guid userId, CancellationToken cancellationToken);

        Task<ReadRequestOrderModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<ReadRequestOrderModel>> GetAllAsync(CancellationToken cancellationToken);

        Task ApproveOrderRequestAsync(Guid id, CancellationToken cancellationToken);

        Task DeclineOrderRequestAsync(Guid id, CancellationToken cancellationToken);
    }
}
