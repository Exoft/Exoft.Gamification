using System.Collections.Generic;

namespace Exoft.Gamification.Api.Data.Core.Helpers
{
    public class ReturnPagingInfo<T>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
