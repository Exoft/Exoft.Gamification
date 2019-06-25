using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RefreshToken
    {
        public DateTime ExpiresUtc { get; set; }

        public string Token { get; set; }

        public Guid UserId { get; set; }
    }
}
