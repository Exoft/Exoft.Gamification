using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models.ReferenceBook
{
    public class CreateChapterModel : BaseChapterModel
    {
        public ICollection<CreateArticleModel> Articles { get; set; }
    }
}
