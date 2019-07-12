using Exoft.Gamification.Api.Common.Helpers;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ResetPasswordModel
    {
        [NonLogged]
        public string Password { get; set; }

        public string SecretString { get; set; }
    }
}
