using AutoMapper;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Threading.Tasks;

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

        public async Task<FileModel> GetFileByIdAsync(Guid Id)
        {
            var file = await _fileRepository.GetByIdAsync(Id);

            return _mapper.Map<FileModel>(file);
        } 
    }
}
