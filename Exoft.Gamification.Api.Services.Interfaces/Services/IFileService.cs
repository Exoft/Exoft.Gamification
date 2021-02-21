using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IFileService
    {
        Task<FileModel> GetFileByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Guid?> AddOrUpdateFileByIdAsync(IFormFile image, Guid? iconId, CancellationToken cancellationToken);
    }
}
