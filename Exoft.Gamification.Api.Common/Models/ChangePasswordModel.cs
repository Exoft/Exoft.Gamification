using Exoft.Gamification.Api.Common.Helpers;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ChangePasswordModel
    {
        [NonLogged]
        public string OldPassword { get; set; }

        [NonLogged]
        public string NewPassword { get; set; }
    }
}
