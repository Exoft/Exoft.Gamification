using System;

namespace Exoft.Gamification.Api.Common.Models
{
    //AZ: given just model name its hard to  understand what it is used for. use Read, Write or Create/Update models
    public class OutAchievementModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public Guid IconId { get; set; }
    }
}
