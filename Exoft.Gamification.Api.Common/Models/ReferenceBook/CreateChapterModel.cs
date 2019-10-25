using System.Collections.Generic;

namespace Exoft.Gamification.Api.Common.Models.ReferenceBook
{
    public class CreateChapterModel : BaseChapterModel
    {
        public ICollection<CreateArticleModel> Articles { get; set; }
    }
}
