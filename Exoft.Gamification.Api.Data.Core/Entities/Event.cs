using Exoft.Gamification.Api.Data.Core.Helpers;
using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Event : Entity
    {
        public Event()
        {
            CreatedTime = DateTime.Now;
        }

        public string Description { get; set; }

        public DateTime CreatedTime { get; private set; }

        public GamificationEnums.EventType Type { get;  set; }

        public User User { get; set; }
    }
}
