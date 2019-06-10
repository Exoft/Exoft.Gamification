using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class UserModel : ResponseModel
    {   
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public int XP { get; set; }

        public string AvatarId { get; set; }
    }
}
