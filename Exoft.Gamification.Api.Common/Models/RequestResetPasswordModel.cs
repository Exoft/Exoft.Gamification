namespace Exoft.Gamification.Api.Common.Models
{
    public class RequestResetPasswordModel
    {
        public string Email { get; set; }

        public string ResetPasswordPageLink { get; set; }
    }
}
