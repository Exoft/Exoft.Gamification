using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class RequestAchievement : Entity
    {
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("AchievementId")]
        public Guid AchievementId { get; set; }

        public string Message { get; set; }

        public User User { get; set; }

        public Achievement Achievement { get; set; }
    }
}
