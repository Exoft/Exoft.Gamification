using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class ReadFullUserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public int XP { get; set; }

        public int BadgetCount { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public Guid? AvatarId { get; set; }
    }
}
