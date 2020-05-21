using System;

namespace Exoft.Gamification.Api.Common.Models.Category
{
    public class ReadCategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid IconId { get; set; }
    }
}
