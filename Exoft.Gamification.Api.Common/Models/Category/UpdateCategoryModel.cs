using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.Category
{
    public class UpdateCategoryModel
    {
        public string Name { get; set; }

        public IFormFile Icon { get; set; }
    }
}
