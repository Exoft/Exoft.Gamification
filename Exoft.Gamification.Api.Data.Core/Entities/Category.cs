using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public Guid? IconId { get; set; }
    }
}
