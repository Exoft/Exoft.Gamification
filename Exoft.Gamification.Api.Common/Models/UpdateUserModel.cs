using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class UpdateUserModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }
    }
}
