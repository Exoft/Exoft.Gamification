using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService
        (
            ICategoryRepository categoryRepository,
            IFileRepository fileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _categoryRepository = categoryRepository;
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
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

        public async Task<ReadCategoryModel> AddCategoryAsync(CreateCategoryModel model)
        {
            var category = _mapper.Map<Category>(model);

            if (model.Icon != null)
            {
                using (var memory = new MemoryStream())
                {
                    await model.Icon.CopyToAsync(memory);

                    var file = new File
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Icon.ContentType
                    };
                    await _fileRepository.AddAsync(file);
                    category.IconId = file.Id;
                }
            }

            await _categoryRepository.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadCategoryModel>(category);
        }

        public async Task<ReadCategoryModel> UpdateCategoryAsync(UpdateCategoryModel model, Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            category.Name = model.Name;

            if (model.Icon != null)
            {
                using (var memory = new MemoryStream())
                {
                    await model.Icon.CopyToAsync(memory);

                    await _fileRepository.Delete(category.IconId);

                    var file = new File
                    {
                        Data = memory.ToArray(),
                        ContentType = model.Icon.ContentType
                    };

                    await _fileRepository.AddAsync(file);

                    category.IconId = file.Id;
                }
            }

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
