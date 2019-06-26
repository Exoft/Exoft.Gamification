using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class RefreshTokenModel
    {
        public Guid UserId { get; set; }

        public string Token { get; set; }
    }
}
