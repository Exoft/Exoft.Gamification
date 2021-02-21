using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ReturnPagingInfo<ReadCategoryModel>> GetAllCategoryAsync(PagingInfo pagingInfo, CancellationToken cancellationToken);

        Task<ReadCategoryModel> AddCategoryAsync(CreateCategoryModel model, CancellationToken cancellationToken);

        Task<ReadCategoryModel> UpdateCategoryAsync(UpdateCategoryModel model, Guid categoryId, CancellationToken cancellationToken);

        Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken);

        Task<ReadCategoryModel> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
