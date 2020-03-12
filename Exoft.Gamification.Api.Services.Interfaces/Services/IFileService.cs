using System;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Common.Models;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IFileService
    {
        Task<FileModel> GetFileByIdAsync(Guid id);
    }
}
