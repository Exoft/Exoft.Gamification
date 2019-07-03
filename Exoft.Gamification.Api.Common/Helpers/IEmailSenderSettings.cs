namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IEmailSenderSettings
    {
        string Email { get; }

        string Password { get; }

        string SmtpClient { get; }

        int Port { get; }

        bool EnableSsl { get; }

        string ResetPasswordPage { get; }
    }
}
