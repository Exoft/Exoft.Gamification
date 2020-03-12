using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserService
    {
        Task<ReadFullUserModel> AddUserAsync(CreateUserModel model);

        Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid userId);

        Task UpdatePasswordAsync(Guid userId, string newPassword);

        Task DeleteUserAsync(Guid id);

        Task<ReadShortUserModel> GetShortUserByIdAsync(Guid id);

        Task<ReadFullUserModel> GetFullUserByIdAsync(Guid id);

        Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUsersWithShortInfoAsync(PagingInfo pagingInfo);

        Task<ReturnPagingInfo<ReadFullUserModel>> GetAllUsersWithFullInfoAsync(PagingInfo pagingInfo);
    }
}
