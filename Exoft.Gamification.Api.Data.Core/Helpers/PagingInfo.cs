namespace Exoft.Gamification.Api.Data.Core.Helpers
{
    public class PagingInfo
    {
        public PagingInfo()
        {
            CurrentPage = 1;
            PageSize = 10;
        }
      
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
