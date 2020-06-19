using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Order : Entity
    {
        public Order()
        {
            Categories = new List<OrderCategory>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Popularity { get; set; }

        public ICollection<OrderCategory> Categories { get; set; }

        public Guid? IconId { get; set; }
    }
}
