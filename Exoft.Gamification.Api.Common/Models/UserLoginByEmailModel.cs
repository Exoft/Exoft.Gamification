using Exoft.Gamification.Api.Common.Helpers;
using System;

namespace Exoft.Gamification.Api.Common.Models
{
    [Obsolete("Wll remove in next release.", false)]
    public class UserLoginByEmailModel
    {
        public string Email { get; set; }

        [NonLogged]
        public string Password { get; set; }
    }
}
