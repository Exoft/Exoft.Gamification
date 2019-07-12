using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public Guid UserId { get; set; }
    }
}
