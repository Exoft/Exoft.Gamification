using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string message, string email);

        Task SendEmailsAsync(string subject, string message, params string[] emails);
    }
}
