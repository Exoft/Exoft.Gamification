using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Thank;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IThankService
    {
        Task AddAsync(CreateThankModel model, Guid fromUserId);

        Task<ReadThankModel> GetLastThankAsync(Guid toUserId);
    }
}
