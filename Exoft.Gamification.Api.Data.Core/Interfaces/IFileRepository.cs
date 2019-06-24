using Exoft.Gamification.Api.Data.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IFileRepository : IRepository<File>
    {
        Task Delete(Guid fileId);
    }
}
