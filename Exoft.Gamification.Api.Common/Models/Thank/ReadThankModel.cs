using System;

namespace Exoft.Gamification.Api.Common.Models.Thank
{
    public class ReadThankModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid? AvatarId { get; set; }

        public Guid? UserId { get; set; }

        public string Text { get; set; }
    }
}
