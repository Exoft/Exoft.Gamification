using Exoft.Gamification.Api.Common.Models.User;

namespace Exoft.Gamification.Api.Common.Models
{
    public class JwtTokenModel : ReadFullUserModel
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string TokenExpiration { get; set; }
    }
}
