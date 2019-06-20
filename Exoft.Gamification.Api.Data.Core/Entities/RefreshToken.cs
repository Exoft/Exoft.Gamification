using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RefreshToken : Entity
    {
        public DateTime ExpiresUtc { get; set; }

        public string Token { get; set; }

        public User User { get; set; }
    }
}
