using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;

        public CategoryService
        (
            ICategoryRepository categoryRepository,
            IMapper mapper
        )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ReturnPagingInfo<ReadCategoryModel>> GetAllCategoryAsync(PagingInfo pagingInfo)
        {
            var page = await _categoryRepository.GetAllDataAsync(pagingInfo);

            var readCategoryModels = page.Data
                .Select(category => _mapper.Map<ReadCategoryModel>(category))
                .ToList();

            var result = new ReturnPagingInfo<ReadCategoryModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readCategoryModels
            };

            return result;
        }
    }
}
