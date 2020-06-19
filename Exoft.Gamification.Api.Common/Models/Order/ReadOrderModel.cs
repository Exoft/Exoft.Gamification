using System;
using System.Collections.Generic;

using Exoft.Gamification.Api.Common.Models.Category;

namespace Exoft.Gamification.Api.Common.Models.Order
{
    public class ReadOrderModel
    {
        public ReadOrderModel()
        {
            Categories = new List<ReadCategoryModel>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Popularity { get; set; }

        public ICollection<ReadCategoryModel> Categories { get; set; }

        public Guid? IconId { get; set; }
    }
}
