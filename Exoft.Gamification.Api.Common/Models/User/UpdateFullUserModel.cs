using System.Collections.Generic;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class UpdateFullUserModel : UpdateUserModel
    {
        public UpdateFullUserModel()
        {
            Roles = new List<string>();
        }

        public ICollection<string> Roles { get; set; }
    }
}
