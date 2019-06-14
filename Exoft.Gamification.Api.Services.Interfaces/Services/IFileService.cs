using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IFileService
    {
        Task<File> GetFileByIdAsync(Guid Id);
    }
}
