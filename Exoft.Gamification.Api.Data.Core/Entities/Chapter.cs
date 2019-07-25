using System.Collections.Generic;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Chapter : Entity
    {
        public string Title { get; set; }

        public ICollection<Article> Articles { get; set; }

        public int OrderId { get; set; }
    }
}
}
