using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }

        public Guid UserId { get; set; }
    }
}
