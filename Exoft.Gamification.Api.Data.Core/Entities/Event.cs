using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Event : Entity
    {
        public string Description { get; set; }

        public DateTime Time { get; set; }

        public Types Type { get; set; }

        public User User { get; set; }
    }

    public enum Types
    {
        Race,
        Records,
        Challenge,
        Upload
    }
}
