using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class UserModel : BaseModel
    {
        public string Email { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Avatar { get; set; }

        public int XP { get; set; }

        public ICollection<AchievementModel> Achievements { get; set; }

        public string Token { get; set; }
    }
}
