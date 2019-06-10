using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class User : Entity
    {
        public User()
        {
            Achievements = new List<UserAchievements>();
            Roles = new List<UserRoles>();
        }

        public string Email { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Status { get; set; }

        public File Avatar { get; set; }
        
        public int XP { get; set; }

        public ICollection<UserRoles> Roles { get; set; }

        public ICollection<UserAchievements> Achievements { get; set; }
    }
}
