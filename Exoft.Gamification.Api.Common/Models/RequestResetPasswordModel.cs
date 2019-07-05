using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class RequestResetPasswordModel
    {
        public string Email { get; set; }

        public Uri ResetPasswordPageLink { get; set; }
    }
}
