using Exoft.Gamification.Api.Common.Models.Thank;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IThankService
    {
        Task AddAsync(CreateThankModel model, Guid fromUserId);
        Task<ReadThankModel> GetThankAsync(Guid toUserId);
    }
}
