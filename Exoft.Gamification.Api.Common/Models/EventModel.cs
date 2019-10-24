using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class EventModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid? AvatarId { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string CreatedTime { get; set; }
    }
}
