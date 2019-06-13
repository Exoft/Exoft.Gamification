using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.Achievement
{
    public class CreateAchievementModel
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int XP { get; set; }
        
        public IFormFile Icon { get; set; }
    }
}
