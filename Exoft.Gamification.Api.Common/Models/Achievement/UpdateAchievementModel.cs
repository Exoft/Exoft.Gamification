using System.ComponentModel.DataAnnotations;

namespace Exoft.Gamification.Api.Common.Models.Achievement
{
    public class UpdateAchievementModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000)]
        public int XP { get; set; }

        //[Required]
        //public IFormFile Icon { get; set; }
    }
}
