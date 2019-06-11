using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
