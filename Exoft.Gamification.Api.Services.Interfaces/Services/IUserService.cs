using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserService
    {
        Task<ReadFullUserModel> AddUserAsync(CreateUserModel model);
        Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid Id);
        Task DeleteUserAsync(Guid Id);
        Task<ReadFullUserModel> GetUserByIdAsync(Guid Id);
        Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUserAsync(PagingInfo pagingInfo);
    }
}
