﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class UserAchievements
    {
        public User User { get; set; }

        public Achievement Achievement { get; set; }
    }
}
