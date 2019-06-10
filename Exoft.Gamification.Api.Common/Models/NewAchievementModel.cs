using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class NewAchievementModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        public string Icon { get; set; }
    }
}
