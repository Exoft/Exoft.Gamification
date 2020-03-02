using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Helpers;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ReturnPagingInfo<ReadCategoryModel>> GetAllCategoryAsync(PagingInfo pagingInfo);
    }
}
