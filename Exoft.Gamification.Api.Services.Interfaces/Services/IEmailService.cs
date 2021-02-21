using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string message, string email, CancellationToken cancellationToken);

        Task SendEmailsAsync(string subject, string message, CancellationToken cancellationToken, params string[] emails);
    }
}
