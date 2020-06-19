using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService
        (
            ICategoryRepository categoryRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReturnPagingInfo<ReadCategoryModel>> GetAllCategoryAsync(PagingInfo pagingInfo)
        {
            var page = await _categoryRepository.GetAllDataAsync(pagingInfo);

            var readCategoryModels = page.Data.Select(category => _mapper.Map<ReadCategoryModel>(category)).ToList();
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

        public async Task<ReadCategoryModel> AddCategoryAsync(CreateCategoryModel model)
        {
            var category = _mapper.Map<Category>(model);
            category.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, category.IconId);

            await _categoryRepository.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadCategoryModel>(category);
        }

        public async Task<ReadCategoryModel> UpdateCategoryAsync(UpdateCategoryModel model, Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            category.Name = model.Name;
            category.IconId = await _fileService.AddOrUpdateFileByIdAsync(model.Icon, category.IconId);

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadCategoryModel>(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            _categoryRepository.Delete(category);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReadCategoryModel> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return _mapper.Map<ReadCategoryModel>(category);
        }
    }
}
