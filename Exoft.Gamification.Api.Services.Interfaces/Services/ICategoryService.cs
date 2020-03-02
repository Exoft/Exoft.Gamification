using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ReturnPagingInfo<ReadCategoryModel>> GetAllCategoryAsync(PagingInfo pagingInfo);

        Task<ReadCategoryModel> AddCategoryAsync(CreateCategoryModel model);

        Task<ReadCategoryModel> UpdateCategoryAsync(UpdateCategoryModel model, Guid id);

        Task DeleteCategoryAsync(Guid id);

        Task<ReadCategoryModel> GetCategoryByIdAsync(Guid id);
    }
}
