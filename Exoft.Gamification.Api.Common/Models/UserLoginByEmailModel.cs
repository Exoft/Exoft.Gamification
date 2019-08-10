using Exoft.Gamification.Api.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class UserLoginByEmailModel
    {
        public string Email { get; set; }

        [NonLogged]
        public string Password { get; set; }
    }
}
