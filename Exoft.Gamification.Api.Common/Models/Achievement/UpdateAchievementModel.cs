using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.Achievement
{
    public class UpdateAchievementModel
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int XP { get; set; }

        [NonLogged]
        public IFormFile Icon { get; set; }
    }
}
