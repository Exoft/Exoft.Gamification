using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class CreateUserModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public string Status { get; set; }

        [NonLogged]
        public IFormFile Avatar { get; set; }
    }
}
