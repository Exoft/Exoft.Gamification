using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using NUnit.Framework;
using System;

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
            //new TestCaseData(null),
            new TestCaseData(new PagingInfo{ CurrentPage = 5, PageSize = -1}),
            new TestCaseData(new PagingInfo{ CurrentPage = -1, PageSize = -10})
        };

        public static readonly TestCaseData[] ValidUserId = new TestCaseData[]
        {
            new TestCaseData(Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] ValidCreateUserModels = new TestCaseData[]
        {
            new TestCaseData(DumbData.GetRandomCreateUserModel())
        };

        public static readonly TestCaseData[] ValidUpdateFullUserModelsWithIds = new TestCaseData[]
        {
            new TestCaseData(DumbData.GetRandomUpdateFullUserModel(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(DumbData.GetRandomUpdateFullUserModel(), Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };
    }
}
