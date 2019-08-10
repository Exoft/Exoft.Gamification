using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models.ReferenceBook
{
    public class CreateArticleModel : BaseArticleModel
    {
        public Guid? IdChapter { get; set; }
    }
}
