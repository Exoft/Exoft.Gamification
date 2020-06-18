using System;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public FileService
        (
            IFileRepository fileRepository,
            IMapper mapper
        )
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<FileModel> GetFileByIdAsync(Guid id)
        {
            var file = await _fileRepository.GetByIdAsync(id);

            return _mapper.Map<FileModel>(file);
        }

        public async Task<Guid?> AddOrUpdateFileByIdAsync(IFormFile image, Guid? IconId)
        {
            if (image == null)
            {
                return IconId;
            }

            using var memory = new System.IO.MemoryStream();
            await image.CopyToAsync(memory);

            if (IconId.HasValue)
            {
                await _fileRepository.Delete(IconId.Value);
            }

            var file = new File
            {
                Data = memory.ToArray(),
                ContentType = image.ContentType
            };
            await _fileRepository.AddAsync(file);
            return file.Id;
        }
    }
}
