using Exoft.Gamification.Api.Common.Models;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRequestAchievementService
    {
        Task<IResponse> AddAsync(RequestAchievementModel model, Guid userId);
    }
}
