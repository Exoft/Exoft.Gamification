using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class UpdateUserModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
