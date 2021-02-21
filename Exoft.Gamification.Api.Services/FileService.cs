using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileService
        (
            IFileRepository fileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FileModel> GetFileByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<FileModel>(file);
        }

        public async Task<Guid?> AddOrUpdateFileByIdAsync(IFormFile image, Guid? iconId, CancellationToken cancellationToken)
        {
            if (image == null)
            {
                return iconId;
            }

            await using var memory = new System.IO.MemoryStream();
            await image.CopyToAsync(memory, cancellationToken);

            if (iconId.HasValue)
            {
                await _fileRepository.Delete(iconId.Value, cancellationToken);
            }

            var file = new File
            {
                Data = memory.ToArray(),
                ContentType = image.ContentType
            };
            await _fileRepository.AddAsync(file, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return file.Id;
        }
    }
}
