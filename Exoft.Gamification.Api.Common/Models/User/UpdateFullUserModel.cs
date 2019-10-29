using System.Collections.Generic;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class UpdateFullUserModel : UpdateUserModel
    {
        public List<string> Roles { get; set; }
    }
}
