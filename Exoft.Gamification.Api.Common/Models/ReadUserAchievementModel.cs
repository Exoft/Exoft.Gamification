using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ReadUserAchievementModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public string AddedTime { get; set; }

        public Guid IconId { get; set; }
    }
}
