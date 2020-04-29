using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.Category
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }

        public IFormFile Icon { get; set; }
    }
}
