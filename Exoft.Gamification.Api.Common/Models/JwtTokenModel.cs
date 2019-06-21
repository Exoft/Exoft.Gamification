using Exoft.Gamification.Api.Common.Models.User;

namespace Exoft.Gamification.Api.Common.Models
{
    public class JwtTokenModel : ReadFullUserModel
    {
        public string Token { get; set; }
    }
}
