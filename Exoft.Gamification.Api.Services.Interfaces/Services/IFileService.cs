using Exoft.Gamification.Api.Common.Models;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IFileService
    {
        Task<FileModel> GetFileByIdAsync(Guid Id);
    }
}
