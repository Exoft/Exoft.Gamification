using System;

namespace Exoft.Gamification.Api.Common.Models.User
{
    public class ReadShortUserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int XP { get; set; }

        public int BadgesCount { get; set; }

        public Guid? AvatarId { get; set; }
    }
}
