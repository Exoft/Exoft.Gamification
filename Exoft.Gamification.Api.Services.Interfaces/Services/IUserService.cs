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
        Task<ReadFullUserModel> UpdateUserAsync(UpdateFullUserModel model, Guid Id);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task DeleteUserAsync(Guid Id);
        Task<ReadShortUserModel> GetShortUserByIdAsync(Guid Id);
        Task<ReadFullUserModel> GetFullUserByIdAsync(Guid Id);
        Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUserAsync(PagingInfo pagingInfo);
    }
}
