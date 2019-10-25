using System;

namespace Exoft.Gamification.Api.Common.Models.ReferenceBook
{
    public class UpdateArticleModel : BaseArticleModel
    {
        public Guid Id { get; set; }
        public double? UnitNumber { get; set; }
    }
}
