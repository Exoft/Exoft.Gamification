using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IUserService
    {
        Task<ReadFullUserModel> AddUserAsync(CreateUserModel model, CancellationToken cancellationToken);

        Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid userId, CancellationToken cancellationToken);

        Task UpdatePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken);

        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);

        Task<ReadShortUserModel> GetShortUserByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ReadFullUserModel> GetFullUserByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUsersWithShortInfoAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);

        Task<ReturnPagingInfo<ReadFullUserModel>> GetAllUsersWithFullInfoAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);
    }
}
