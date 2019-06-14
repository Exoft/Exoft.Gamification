using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<File> GetFileByIdAsync(Guid Id)
        {
            return await _fileRepository.GetByIdAsync(Id);
        }
    }
}
