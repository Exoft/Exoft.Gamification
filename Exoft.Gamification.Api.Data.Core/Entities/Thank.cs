using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class Thank : Entity
    {
        public Guid ToUserId { get; set; }

        public Guid FromUserId { get; set; }

        public string Text { get; set; }
    }
}
