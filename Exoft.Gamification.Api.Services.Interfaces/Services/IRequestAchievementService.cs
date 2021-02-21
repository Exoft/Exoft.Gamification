using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRequestAchievementService
    {
        Task<IResponse> AddAsync(CreateRequestAchievementModel model, Guid userId, CancellationToken cancellationToken);

        Task<RequestAchievement> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<ReadRequestAchievementModel>> GetAllAsync(CancellationToken cancellationToken);

        Task ApproveAchievementRequestAsync(Guid id, CancellationToken cancellationToken);

        Task DeleteAsync(RequestAchievement achievementRequest, CancellationToken cancellationToken);
    }
}
