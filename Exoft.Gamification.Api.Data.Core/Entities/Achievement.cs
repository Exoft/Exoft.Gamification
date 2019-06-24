using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Achievement : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public Guid? IconId { get; set; }
    }
}
