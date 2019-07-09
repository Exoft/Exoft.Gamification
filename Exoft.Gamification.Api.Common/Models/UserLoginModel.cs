using Exoft.Gamification.Api.Common.Helpers;

namespace Exoft.Gamification.Api.Common.Models
{
    public class UserLoginModel
    {
        public string UserName { get; set; }

        [NonLogged]
        public string Password { get; set; }
    }
}
