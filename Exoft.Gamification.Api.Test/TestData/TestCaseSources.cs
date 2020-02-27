using Exoft.Gamification.Api.Data.Core.Helpers;
using NUnit.Framework;

namespace Exoft.Gamification.Api.Test.TestData
{
    public static class TestCaseSources
    {
        public static readonly TestCaseData[] ValidPagingInfos = new TestCaseData[]
        {
            new TestCaseData(new PagingInfo()),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 1}),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 10})
        };

        public static readonly TestCaseData[] InvalidPagingInfos = new TestCaseData[]
        {
            new TestCaseData(new PagingInfo{ CurrentPage = 5, PageSize = -1}),
            new TestCaseData(new PagingInfo{ CurrentPage = -1, PageSize = -10})
        };
    }
}
