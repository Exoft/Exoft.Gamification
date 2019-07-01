using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Thank : Entity
    {
        public Thank()
        {
            AddedTime = DateTime.UtcNow;
        }

        public Guid ToUserId { get; set; }

        public User FromUser { get; set; }

        public string Text { get; set; }

        public DateTime AddedTime { get; private set; }
    }
}
