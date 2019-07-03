namespace Exoft.Gamification.Api.Common.Models
{
    public class ResetPasswordModel
    {
        public string newPassword { get; set; }

        public string secretString { get; set; }
    }
}
