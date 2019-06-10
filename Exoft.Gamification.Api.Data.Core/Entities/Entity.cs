using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
