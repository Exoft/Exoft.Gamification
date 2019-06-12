namespace Exoft.Gamification.Api.Common.Models
{
    //AZ: better to  group models in folders
    //AZ: given just model name its hard to  understand what it is used for. use Read, Write or Create/Update models
    public class InAchievementModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int XP { get; set; }

        //public IFormFile Icon { get; set; }
    }
}
