﻿namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievement : Entity
    {
        public User User { get; set; }

        public Achievement Achievement { get; set; }
    }
}