using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ReturnPageModel<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
