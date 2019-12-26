using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models.RequestAchievement
{
    public class ReadRequestAchievementModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public string AchievementName { get; set; }

        public string UserName { get; set; }

        public Guid AchievementId { get; set; }

        public Guid UserId { get; set; }
    }
}
