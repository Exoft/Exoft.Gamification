using System;

namespace Exoft.Gamification.Api.Common.Models.ReferenceBook
{
    public class CreateArticleModel : BaseArticleModel
    {
        public Guid? ChapterId { get; set; }
    }
}
